using PipServices.Commons.Commands;
using PipServices.Commons.Data;
using PipServices.Commons.Config;
using PipServices.Commons.Validate;
using System;
using System.Collections.Generic;
using System.Text;
using PipServices.Settings.Logic;

namespace PipServices.Settings.Logic
{
    class SettingsCommandSet : CommandSet
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
                    FilterParams filter = FilterParams.FromValue(args.Get("filter"));
                    PagingParams paging = PagingParams.FromValue(args.Get("paging"));
                    return await _logic.getSectionIds(correlationId, filter, paging);
                });
        }

        private ICommand makeGetSectionsCommand() {
		return new Command(
			"get_sections",

            new ObjectSchema()
				.WithOptionalProperty("filter", new FilterParamsSchema())
				.WithOptionalProperty("paging", new PagingParamsSchema()),
			async (correlationId, args) => 
            {
                FilterParams filter = FilterParams.FromValue(args.Get("filter"));
                PagingParams paging = PagingParams.FromValue(args.Get("paging"));

                return await _logic.getSections(correlationId, filter, paging);
			}
		);
	}

    private ICommand makeGetSectionByIdCommand() {
		return new Command(
			"get_section_by_id",

            new ObjectSchema()
				.WithRequiredProperty("id", TypeCode.String),
            async (correlationId, args) => 
            {
                String Id = args.GetAsNullableString("id");
                return await _logic.getSectionById(correlationId, Id);
            }
		);
	}

	private ICommand makeSetSectionCommand() {
		return new Command(
			"set_section",

            new ObjectSchema()
				.WithRequiredProperty("id", TypeCode.String)
				.WithRequiredProperty("parameters", TypeCode.Object),
            async (correlationId, args) => 
            {
                String id = args.GetAsNullableString("id");
                var parameters = ConfigParams.FromValue(args.GetAsObject("parameters"));
                return await _logic.setSection(correlationId, id, parameters);
            }
		);
	}

	private makeModifySectionCommand() : ICommand {
		return new Command(
			"modify_section",

            new ObjectSchema(true)
				.withRequiredProperty('id', TypeCode.String)
				.withOptionalProperty('update_parameters', TypeCode.Map)
				.withOptionalProperty('increment_parameters', TypeCode.Map),
            (correlationId: string, args: Parameters, callback: (err: any, result: any) => void) => {
                var id = args.getAsNullableString("id");
var updateParams = ConfigParams.fromValue(args.getAsObject("update_params"));
var incrementParams = ConfigParams.fromValue(args.getAsObject("increment_params"));
                this._logic.modifySection(correlationId, id, updateParams, incrementParams, callback);
            }
		);
	}

       
    }
}
