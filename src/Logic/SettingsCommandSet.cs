using PipServices.Commons.Commands;
using PipServices.Commons.Data;
using PipServices.Commons.Validate;
using System;
using System.Collections.Generic;
using System.Text;

namespace PipServices.Settings.Logic
{
    class SettingsCommandSet
    {

        private ISettingsController _logic;

        public SettingsCommandSet(ISettingsController logic)
        {
            _logic = logic;

            AddCommand(makeGetSectionIdsCommand());
        }

        private void AddCommand(ICommand command)
        {
            throw new NotImplementedException();
        }

        private ICommand makeGetSectionIdsCommand()
        {
            return new Command(
                "get_section_ids",
                new ObjectSchema()
                 .WithOptionalProperty("filter", new FilterParamsSchema())
                 .WithOptionalProperty("paging", new PagingParamsSchema()),
                async (correlationId, args) =>
                {
                    var filter = FilterParams.FromValue(args.Get("filter"));
                    var paging = PagingParams.FromValue(args.Get("paging"));
                    return await _logic.getSectionIds(correlationId, filter, paging, callback);
                });

            /*
             new ObjectSchema(true)
				.withOptionalProperty('filter', new FilterParamsSchema())
				.withOptionalProperty('paging', new PagingParamsSchema()),
			(correlationId: string, args: Parameters, callback: (err: any, result: any) => void) => {
				let filter = FilterParams.fromValue(args.get("filter"));
				let paging = PagingParams.fromValue(args.get("paging"));
				this._logic.getSectionIds(correlationId, filter, paging, callback);
			}*/
        }

       
    }
}
