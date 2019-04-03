// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

module App.Controllers {
    'use strict';

    export abstract class PagedController<ScopeType extends Scopes.PagedScope> extends Controller {

        constructor(public $scope: ScopeType) {
            super($scope);

            // Default values
            $scope.PageHeader = [];
            $scope.PageSizes = [5, 10, 25, 50, 100];
            
            // Watch the page properties
            var properties = [
                (i: Scopes.PagedScope) => i.Query.PageNumber,
                (i: Scopes.PagedScope) => i.Query.PageSize,
                (i: Scopes.PagedScope) => i.PageCount
            ];

            $scope.$watchGroup(properties, (newValues: any, oldValues: any, scope: Scopes.PagedScope) => {
                if (!scope || !scope.Query)
                    return;

                var pageNumber = scope.Query.PageNumber;
                var pageCount = scope.PageCount;

                scope.PageHeader = this.GetPages(pageNumber, pageCount);
                scope.IsFirst = (pageNumber == 1);
                scope.IsLast = (pageNumber == pageCount);
            });
        }

         // Metodo abstrato de busca
        abstract Search(): void;

        // Ordenacao
        public Sort(columnName: string) {
            var query = this.$scope.Query;

            if (query.SortField != columnName) {
                query.SortField = columnName;
                query.SortDescending = false;
            }
            else {
                query.SortDescending = !query.SortDescending;
            }

            this.Search();
        }

        public GoTo(pageNumber: number) {
            var query = this.$scope.Query;

            if (pageNumber <= this.$scope.PageCount) {
                query.PageNumber = pageNumber;
                this.Search();
            }
        }

        public Previous() {
            var query = this.$scope.Query;

            if (query.PageNumber > 1) {
                var pageIndex = query.PageNumber - 1;
                this.GoTo(pageIndex);
            }
        }

        public Next() {
            var query = this.$scope.Query;

            if (query.PageNumber < this.$scope.PageCount) {
                var pageIndex = query.PageNumber + 1;
                this.GoTo(pageIndex);
            }
        }

        public First() {
            if (this.$scope.PageCount > 0) {
                this.GoTo(1);
            }
        }

        public Last() {
            if (this.$scope.PageCount > 0) {
                this.GoTo(this.$scope.PageCount);
            }
        }

        private GetPages(pageIndex: number, pageCount: number): number[] {
            var list = new Array<number>();

            if (pageCount == 0)
                return list;

            // Numero maximo de paginas a serem exibidas
            var count = 5;
            var offset = count - 1;

            var split = Math.floor(count / 2);
            var min = pageIndex - split;
            var max = min + offset;

            if (min < 1) {
                min = 1;
                max = min + offset;
            }
            else if (max > pageCount) {
                max = pageCount;
                min = max - offset;
            }

            for (var i = min; i <= max; i++) {
                if (i > 0 && i <= pageCount)
                    list.push(i);
            }

            return list;
        }

        public ActiveStyle(active: boolean) {
            var color = active ? 'green' : 'red';
            return 'fa fa-circle isActive situacao-' + color;
        }

        public PageStyle(current: number, selected: number) {
            var style = current == selected ? 'btn btn-primary' : 'btn btn-default';
            return style;
        }

        public PreviousStyle() {
            // var style = this.$scope.Page.Number == 1 ? ' disabled' : '';
            var style = this.$scope.IsFirst ? ' disabled' : '';
            return 'btn btn-default' + style;
        }

        public NextStyle() {
            // var style = this.$scope.Page.Number == this.$scope.Page.Count ? ' disabled' : '';
            var style = this.$scope.IsLast ? ' disabled' : '';
            return 'btn btn-default' + style;
        }

        public HeaderStyle(columnName: string) {
            var query = this.$scope.Query;
            var style = 'tablesorter-header';

            if (columnName == query.SortField) {
                if (query.SortDescending)
                    style += ' tablesorter-headerDesc';
                else
                    style += ' tablesorter-headerAsc'
            }

            return style;
        }
    }
} 
