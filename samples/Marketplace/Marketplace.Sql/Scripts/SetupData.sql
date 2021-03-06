-- Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
-- See License.txt in the project root for license information.

/***************************************
* Roles
***************************************/

BEGIN
	PRINT 'Roles';

	DECLARE @Roles TABLE
	(
		Name	VARCHAR(512),
		IsAdmin	BIT 
	);

	-- Source data
	INSERT INTO @Roles(Name, IsAdmin)
	SELECT 'Admin', 1 UNION 
	SELECT 'Dealer', 0 UNION 
	SELECT 'Customer', 0;

	UPDATE D SET
		D.IsAdmin = O.IsAdmin
	FROM
		Roles D
		INNER JOIN @Roles O ON O.Name = D.Name;

	INSERT INTO [Roles]
	(
		Name, 
		IsAdmin
	)
	SELECT 
		Name, 
		IsAdmin
	FROM
		@Roles
	WHERE
		Name NOT IN (SELECT Name FROM [Roles]);
END;

/***************************************
* Features
***************************************/
BEGIN
	PRINT 'Features';
	
	DELETE FROM Permissions;
	DELETE FROM Features;

	DECLARE @Features TABLE
	(
		Name		VARCHAR(512),
		Detail		VARCHAR(512),
		[Path]		VARCHAR(512),
		ParentName	VARCHAR(512)
	);

	INSERT INTO @Features([Name], Detail, [Path], ParentName)
	SELECT 'Role', '', 'api/Role', '' UNION ALL
	SELECT 'Get Role', '', 'api/Role/Get', 'Role' UNION ALL
	SELECT 'Add Role', '', 'api/Role/Post', 'Role' UNION ALL
	SELECT 'Edit Role', '', 'api/Role/Put', 'Role' UNION ALL
	SELECT 'Delete Role', '', 'api/Role/Delete', 'Role' UNION ALL
	SELECT 'User', '', 'api/User', '' UNION ALL
	SELECT 'Get User', '', 'api/User/Get', 'User' UNION ALL
	SELECT 'Add User', '', 'api/User/Post', 'User' UNION ALL
	SELECT 'Edit User', '', 'api/User/Put', 'User' UNION ALL
	SELECT 'Delete User', '', 'api/User/Delete', 'User' UNION ALL

	SELECT 'Category', '', 'api/Category', '' UNION ALL
	SELECT 'Get Category', '', 'api/Category/Get', 'Category' UNION ALL
	SELECT 'Add Category', '', 'api/Category/Post', 'Category' UNION ALL
	SELECT 'Edit Category', '', 'api/Category/Put', 'Category' UNION ALL
	SELECT 'Delete Category', '', 'api/Category/Delete', 'Category' UNION ALL
	SELECT 'StatusType', '', 'api/StatusType', '' UNION ALL
	SELECT 'Get StatusType', '', 'api/StatusType/Get', 'StatusType' UNION ALL
	SELECT 'Add StatusType', '', 'api/StatusType/Post', 'StatusType' UNION ALL
	SELECT 'Edit StatusType', '', 'api/StatusType/Put', 'StatusType' UNION ALL
	SELECT 'Delete StatusType', '', 'api/StatusType/Delete', 'StatusType' UNION ALL

	SELECT 'Country', '', 'api/Country', '' UNION ALL
	SELECT 'Get Country', '', 'api/Country/Get', 'Country' UNION ALL
	SELECT 'Add Country', '', 'api/Country/Post', 'Country' UNION ALL
	SELECT 'Edit Country', '', 'api/Country/Put', 'Country' UNION ALL
	SELECT 'Delete Country', '', 'api/Country/Delete', 'Country' UNION ALL
	SELECT 'Province', '', 'api/Province', '' UNION ALL
	SELECT 'Get Province', '', 'api/Province/Get', 'Province' UNION ALL
	SELECT 'Add Province', '', 'api/Province/Post', 'Province' UNION ALL
	SELECT 'Edit Province', '', 'api/Province/Put', 'Province' UNION ALL
	SELECT 'Delete Province', '', 'api/Province/Delete', 'Province' UNION ALL
	
	SELECT 'Customer', '', 'api/Customer', '' UNION ALL
	SELECT 'Get Customer', '', 'api/Customer/Get', 'Customer' UNION ALL
	SELECT 'Add Customer', '', 'api/Customer/Post', 'Customer' UNION ALL
	SELECT 'Edit Customer', '', 'api/Customer/Put', 'Customer' UNION ALL
	SELECT 'Delete Customer', '', 'api/Customer/Delete', 'Customer' UNION ALL
	SELECT 'Dealer', '', 'api/Dealer', '' UNION ALL
	SELECT 'Get Dealer', '', 'api/Dealer/Get', 'Dealer' UNION ALL
	SELECT 'Add Dealer', '', 'api/Dealer/Post', 'Dealer' UNION ALL
	SELECT 'Edit Dealer', '', 'api/Dealer/Put', 'Dealer' UNION ALL
	SELECT 'Delete Dealer', '', 'api/Dealer/Delete', 'Dealer' UNION ALL
	SELECT 'Product', '', 'api/Product', '' UNION ALL
	SELECT 'Get Product', '', 'api/Product/Get', 'Product' UNION ALL
	SELECT 'Add Product', '', 'api/Product/Post', 'Product' UNION ALL
	SELECT 'Edit Product', '', 'api/Product/Put', 'Product' UNION ALL
	SELECT 'Delete Product', '', 'api/Product/Delete', 'Product' UNION ALL
	SELECT 'Order', '', 'api/Order', '' UNION ALL
	SELECT 'Get Order', '', 'api/Order/Get', 'Order' UNION ALL
	SELECT 'Add Order', '', 'api/Order/Post', 'Order' UNION ALL
	SELECT 'Edit Order', '', 'api/Order/Put', 'Order' UNION ALL
	SELECT 'Delete Order', '', 'api/Order/Delete', 'Order';

	INSERT INTO Features (ParentId, [Name], Detail, [Path])
	SELECT 
		NULL, 
		S.[Name], 
		'', 
		S.[Path]
	FROM 
		@Features S
	WHERE 
		S.ParentName = '';

	INSERT INTO Features (ParentId, [Name], Detail, [Path])
	SELECT 
		F.Id, 
		S.[Name], 
		'',
		S.[Path]
	FROM 
		@Features S
		INNER JOIN Features F ON F.[Name] = S.ParentName;
END;

/***************************************
* Permissions
***************************************/
BEGIN
	PRINT 'Permissions';

	DECLARE @Permissions TABLE
	(
		RoleName	VARCHAR(512),
		FeaturePath	VARCHAR(512)
	);

	INSERT INTO @Permissions(RoleName, FeaturePath)
	SELECT 'Customer', [Path]
	FROM @Features
	WHERE [Path] LIKE '%/Get';

	INSERT INTO @Permissions(RoleName, FeaturePath)
	SELECT 'Dealer', [Path]
	FROM @Features
	WHERE ParentName <> '';
	
	INSERT INTO Permissions
	(
		RoleId, 
		FeatureId
	)
	SELECT 
		P.Id, 
		F.Id
	FROM
		@Permissions O
		INNER JOIN Roles P ON O.RoleName = P.Name
		INNER JOIN Features F ON O.FeaturePath = F.[Path];
END;

/***************************************
* Countries
***************************************/

BEGIN
	PRINT 'Countries';

	WITH Source (Name, Abbreviation) AS
	(	
		SELECT 'Brasil', 'BR' UNION
		SELECT 'Unites States', 'US'
	)
	INSERT INTO Countries
	(
		Name, 
		Abbreviation
	)
	SELECT 
		Name, 
		Abbreviation
	FROM
		Source
	WHERE
		Name NOT IN (SELECT Name FROM Countries);
END;

/***************************************
* Provinces
***************************************/

BEGIN
	PRINT 'Provinces';

	DECLARE @Provinces TABLE
	(
		Country	VARCHAR(512),
		[Name]	VARCHAR(512),
		[Abbr]	VARCHAR(512)
	);

	INSERT INTO @Provinces(Country, [Name], Abbr)
	SELECT 'BR', 'Acre', 'AC' UNION
	SELECT 'BR', 'Alagoas', 'AL' UNION
	SELECT 'BR', 'Amapá', 'AP' UNION
	SELECT 'BR', 'Amazonas', 'AM' UNION
	SELECT 'BR', 'Bahia', 'BA' UNION
	SELECT 'BR', 'Ceará', 'CE' UNION
	SELECT 'BR', 'Distrito Federal', 'DF' UNION
	SELECT 'BR', 'Espírito Santo', 'ES' UNION
	SELECT 'BR', 'Goiás', 'GO' UNION
	SELECT 'BR', 'Maranhão', 'MA' UNION
	SELECT 'BR', 'Mato Grosso', 'MT' UNION
	SELECT 'BR', 'Mato Grosso do Sul', 'MS' UNION
	SELECT 'BR', 'Minas Gerais', 'MG' UNION
	SELECT 'BR', 'Pará', 'PA' UNION
	SELECT 'BR', 'Paraíba', 'PB' UNION
	SELECT 'BR', 'Paraná', 'PR' UNION
	SELECT 'BR', 'Pernambuco', 'PE' UNION
	SELECT 'BR', 'Piauí', 'PI' UNION
	SELECT 'BR', 'Rio de Janeiro', 'RJ' UNION
	SELECT 'BR', 'Rio Grande do Norte', 'RN' UNION
	SELECT 'BR', 'Rio Grande do Sul', 'RS' UNION
	SELECT 'BR', 'Rondônia', 'RO' UNION
	SELECT 'BR', 'Roraima', 'RR' UNION
	SELECT 'BR', 'São Paulo', 'SP' UNION
	SELECT 'BR', 'Santa Catarina', 'SP' UNION
	SELECT 'BR', 'Sergipe', 'SE' UNION
	SELECT 'BR', 'Tocantins', 'TO' UNION
	SELECT 'US', 'Alabama', 'AL' UNION
	SELECT 'US', 'Alaska', 'AK' UNION
	SELECT 'US', 'Arizona', 'AZ' UNION
	SELECT 'US', 'Arkansas', 'AR' UNION
	SELECT 'US', 'California', 'CA' UNION
	SELECT 'US', 'Colorado', 'CO' UNION
	SELECT 'US', 'Connecticut', 'CT' UNION
	SELECT 'US', 'Delaware', 'DE' UNION
	SELECT 'US', 'Florida', 'FL' UNION
	SELECT 'US', 'Georgia', 'GA' UNION
	SELECT 'US', 'Hawaii', 'HI' UNION
	SELECT 'US', 'Idaho', 'ID' UNION
	SELECT 'US', 'Illinois', 'IL' UNION
	SELECT 'US', 'Indiana', 'IN' UNION
	SELECT 'US', 'Iowa', 'IA' UNION
	SELECT 'US', 'Kansas', 'KS' UNION
	SELECT 'US', 'Kentucky', 'KY' UNION
	SELECT 'US', 'Louisiana', 'LA' UNION
	SELECT 'US', 'Maine', 'ME' UNION
	SELECT 'US', 'Maryland', 'MD' UNION
	SELECT 'US', 'Massachusetts', 'MA' UNION
	SELECT 'US', 'Michigan', 'MI' UNION
	SELECT 'US', 'Minnesota', 'MN' UNION
	SELECT 'US', 'Mississippi', 'MS' UNION
	SELECT 'US', 'Missouri', 'MO' UNION
	SELECT 'US', 'Montana', 'MT' UNION
	SELECT 'US', 'Nebraska', 'NE' UNION
	SELECT 'US', 'Nevada', 'NV' UNION
	SELECT 'US', 'New Hampshire', 'NH' UNION
	SELECT 'US', 'New Jersey', 'NJ' UNION
	SELECT 'US', 'New Mexico', 'NM' UNION
	SELECT 'US', 'New York', 'NY' UNION
	SELECT 'US', 'North Carolina', 'NC' UNION
	SELECT 'US', 'North Dakota', 'ND' UNION
	SELECT 'US', 'Ohio', 'OH' UNION
	SELECT 'US', 'Oklahoma', 'OK' UNION
	SELECT 'US', 'Oregon', 'OR' UNION
	SELECT 'US', 'Pennsylvania', 'PA' UNION
	SELECT 'US', 'Rhode Island', 'RI' UNION
	SELECT 'US', 'South Carolina', 'SC' UNION
	SELECT 'US', 'South Dakota', 'SD' UNION
	SELECT 'US', 'Tennessee', 'TN' UNION
	SELECT 'US', 'Texas', 'TX' UNION
	SELECT 'US', 'Utah', 'UT' UNION
	SELECT 'US', 'Vermont', 'VT' UNION
	SELECT 'US', 'Virginia', 'VA' UNION
	SELECT 'US', 'Washington', 'WA' UNION
	SELECT 'US', 'West Virginia', 'WV' UNION
	SELECT 'US', 'Wisconsin', 'WI' UNION
	SELECT 'US', 'Wyoming', 'WY';
	
	INSERT INTO Provinces
	(
		Name, 
		Abbreviation,
		CountryId
	)
	SELECT 
		S.Name, 
		S.Abbr,
		C.ID
	FROM
		@Provinces S
		INNER JOIN Countries C ON C.Abbreviation = S.Country
		LEFT JOIN Provinces P ON P.CountryId = C.Id AND P.Abbreviation = S.Abbr
	WHERE
		P.Id IS NULL;
END;

/***************************************
* Settings
***************************************/

BEGIN
	PRINT 'Settings';

	DECLARE @Settings TABLE
	(
		Name	VARCHAR(512),
		Value	VARCHAR(512)
	);

	INSERT INTO @Settings(Name, Value)
	SELECT 'CustomerRole', 'Customer' UNION 
	SELECT 'DealerRole', 'Dealer'

	UPDATE D SET
		D.Value = O.Value
	FROM
		Settings D
		INNER JOIN @Settings O ON O.Name = D.Name;

	INSERT INTO [Settings]
	(
		Name, 
		Value
	)
	SELECT 
		Name, 
		Value
	FROM
		@Settings
	WHERE
		Name NOT IN (SELECT Name FROM [Settings]);
END;

/***************************************
* Status Types
***************************************/

BEGIN
	PRINT 'Status Types';

	DECLARE @StatusTypes TABLE
	(
		Name	VARCHAR(512)
	);

	INSERT INTO @StatusTypes(Name)
	SELECT 'Opened' UNION 
	SELECT 'Pending' UNION 
	SELECT 'Close' UNION 
	SELECT 'Cancelled';

	INSERT INTO StatusTypes
	(
		Name
	)
	SELECT 
		Name
	FROM
		@StatusTypes
	WHERE
		Name NOT IN (SELECT Name FROM [StatusTypes]);
END;
