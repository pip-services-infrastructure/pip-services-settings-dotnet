using PipServices.Commons.Commands;
using PipServices.Commons.Data;
using PipServices.Commons.Config;
using PipServices.Commons.Validate;

using System.Collections.Generic;
using System.Text;
using PipServices.Settings.Logic;
using PipServices.Commons.Convert;

namespace PipServices.Settings.Logic
{
    public class SettingsCommandSet : CommandSet
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
                    string Id = args.GetAsNullableString("id");
                    return await _logic.GetSectionByIdAsync(correlationId, Id);
                }
            );
        }

        private ICommand MakeClearCommand()
        {
            return new Command(
                "clear",

                new ObjectSchema(),
                (correlationId, args) =>
                {
                    return _logic.ClearAsync(correlationId);
                }
            );
        }

        private ICommand MakeSetSectionCommand()
        {
            return new Command(
                "set_section",

                new ObjectSchema()
                    .WithRequiredProperty("id", TypeCode.String)
                    .WithRequiredProperty("parameters", null),
                async (correlationId, args) =>
                {
                    string id = args.GetAsNullableString("id");
                    Dictionary<string, dynamic> parameters = args.GetAsParameters("parameters");
                    return await _logic.SetSectionAsync(correlationId, id, parameters);
                }
            );
        }

        private ICommand MakeDeleteQuoteByIdCommand()
        {
            return new Command(
                "delete_settings_by_id",
                new ObjectSchema()
                    .WithOptionalProperty("id", TypeCode.String),
                async (correlationId, parameters) =>
                {
                    var id = parameters.GetAsString("id");
                    return await _logic.DeleteSectionByIdAsync(correlationId, id);
                });
        }

        private ICommand MakeModifySectionCommand()
        {
            return new Command(
                "modify_section",

                new ObjectSchema()
                    .WithRequiredProperty("id", TypeCode.String)
                    .WithOptionalProperty("update_params", null)
                    .WithOptionalProperty("increment_params", null),
                async (correlationId, args) =>
                {
                    string id = args.GetAsNullableString("id");
                    Dictionary<string, dynamic> updateParams = args.GetAsParameters("update_params");
                    Dictionary<string, dynamic> incrementParams = args.GetAsParameters("increment_params");
                    return await _logic.ModifySectionAsync(correlationId, id, updateParams, incrementParams);
                }
            );
        }


    }
}
