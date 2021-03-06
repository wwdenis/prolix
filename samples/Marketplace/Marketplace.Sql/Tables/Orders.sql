-- Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
-- See License.txt in the project root for license information.

CREATE TABLE Orders
(
	Id			INT IDENTITY NOT NULL,
	Active		BIT NOT NULL DEFAULT 1,

	[Date]			DATETIME NOT NULL,
	[TotalAmount]	NUMERIC(15, 3) NOT NULL DEFAULT 0,
	
	StatusId	INT NOT NULL,
	DealerId	INT NOT NULL,
	CustomerId	INT NOT NULL,

	CONSTRAINT PK_Orders PRIMARY KEY (Id),
	CONSTRAINT FK_Orders_StatusTypes FOREIGN KEY (StatusId) REFERENCES StatusTypes (Id),
	CONSTRAINT FK_Orders_Dealers FOREIGN KEY (DealerId) REFERENCES Dealers (Id),
	CONSTRAINT FK_Orders_Customer FOREIGN KEY (CustomerId) REFERENCES Customers (Id)
)
