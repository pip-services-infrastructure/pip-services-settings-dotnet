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

            AddCommand(MakeGetSectionIdsCommand());
            AddCommand(MakeGetSectionsCommand());
            AddCommand(MakeGetSectionByIdCommand());
            AddCommand(MakeSetSectionCommand());
            AddCommand(MakeModifySectionCommand());
        }

        private ICommand MakeGetSectionIdsCommand()
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
                    return await _logic.GetSectionIdsAsync(correlationId, filter, paging);
                });
        }

        private ICommand MakeGetSectionsCommand()
        {
            return new Command(
                "get_sections",

                new ObjectSchema()
                    .WithOptionalProperty("filter", new FilterParamsSchema())
                    .WithOptionalProperty("paging", new PagingParamsSchema()),
                async (correlationId, args) =>
                {
                    FilterParams filter = FilterParams.FromValue(args.Get("filter"));
                    PagingParams paging = PagingParams.FromValue(args.Get("paging"));

                    return await _logic.GetSectionsAsync(correlationId, filter, paging);
                }
            );
        }

        private ICommand MakeGetSectionByIdCommand()
        {
            return new Command(
                "get_section_by_id",

                new ObjectSchema()
                    .WithRequiredProperty("id", TypeCode.String),
                async (correlationId, args) =>
                {
                    String Id = args.GetAsNullableString("id");
                    return await _logic.GetSectionByIdAsync(correlationId, Id);
                }
            );
        }

        private ICommand MakeSetSectionCommand()
        {
            return new Command(
                "set_section",

                new ObjectSchema()
                    .WithRequiredProperty("id", TypeCode.String)
                    .WithRequiredProperty("parameters", TypeCode.Object),
                async (correlationId, args) =>
                {
                    String id = args.GetAsNullableString("id");
                    ConfigParams parameters = ConfigParams.FromValue(args.GetAsObject("parameters"));
                    return await _logic.SetSectionAsync(correlationId, id, parameters);
                }
            );
        }

        private ICommand MakeModifySectionCommand()
        {
            return new Command(
                "modify_section",

                new ObjectSchema()
                    .WithRequiredProperty("id", TypeCode.String)
                    .WithOptionalProperty("update_parameters", TypeCode.Object)
                    .WithOptionalProperty("increment_parameters", TypeCode.Object),
                async (correlationId, args) =>
                {
                    string id = args.GetAsNullableString("id");
                    ConfigParams updateParams = ConfigParams.FromValue(args.GetAsObject("update_params"));
                    ConfigParams incrementParams = ConfigParams.FromValue(args.GetAsObject("increment_params"));
                    return await _logic.ModifySectionAsync(correlationId, id, updateParams, incrementParams);
                }
            );
        }


    }
}
