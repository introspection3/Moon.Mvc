using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace System.Web.WebPages.Html {
    public partial class HtmlHelper {
        public IHtmlString ValidationMessage(string name) {
            return ValidationMessage(name, null, null);
        }

        public IHtmlString ValidationMessage(string name, string message) {
            return ValidationMessage(name, message, (IDictionary<string, object>)null);
        }

        public IHtmlString ValidationMessage(string name, object htmlAttributes) {
            return ValidationMessage(name, null, ObjectToDictionary(htmlAttributes));
        }

        public IHtmlString ValidationMessage(string name, IDictionary<string, object> htmlAttributes) {
            return ValidationMessage(name, null, htmlAttributes);
        }

        public IHtmlString ValidationMessage(string name, string message, object htmlAttributes) {
            return ValidationMessage(name, message, ObjectToDictionary(htmlAttributes));
        }

        public IHtmlString ValidationMessage(string name, string message, IDictionary<string, object> htmlAttributes) {
            if (string.IsNullOrEmpty(name)) {
                throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, "name");
            }
            return BuildValidationMessage(name, message, htmlAttributes);
        }

        private IHtmlString BuildValidationMessage(string name, string message, IDictionary<string, object> htmlAttributes) {
            if (ModelState.IsValidField(name)) {
                return null;
            }
            else {
                IEnumerable<string> errors = ModelState[name].Errors;
                var error = message ?? errors.FirstOrDefault();
                if (error == null) {
                    return null;
                }

                TagBuilder tagBuilder = new TagBuilder("span") { InnerHtml = Encode(error) };
                tagBuilder.MergeAttributes(htmlAttributes);
                tagBuilder.AddCssClass(ValidationInputCssClassName);
                return tagBuilder.ToHtmlString(TagRenderMode.Normal);
            }
        }

        public IHtmlString ValidationSummary() {
            return BuildValidationSummary(message: null, excludeFieldErrors: null, htmlAttributes: (IDictionary<string, object>) null);
        }

        public IHtmlString ValidationSummary(string message) {
            return BuildValidationSummary(message: message, excludeFieldErrors: null, htmlAttributes: (IDictionary<string, object>)null);
        }

        public IHtmlString ValidationSummary(bool excludeFieldErrors) {
            return ValidationSummary(message: null, excludeFieldErrors: excludeFieldErrors, htmlAttributes: (IDictionary<string, object>)null);
        }

        public IHtmlString ValidationSummary(object htmlAttributes) {
            return ValidationSummary(message: null, excludeFieldErrors: false, htmlAttributes: htmlAttributes);
        }

        public IHtmlString ValidationSummary(IDictionary<string, object> htmlAttributes) {
            return ValidationSummary(message: null, excludeFieldErrors: false, htmlAttributes: htmlAttributes);
        }

        public IHtmlString ValidationSummary(string message, object htmlAttributes) {
            return ValidationSummary(message, excludeFieldErrors: false, htmlAttributes: htmlAttributes);
        }

        public IHtmlString ValidationSummary(string message, IDictionary<string, object> htmlAttributes) {
            return ValidationSummary(message, excludeFieldErrors: false, htmlAttributes: htmlAttributes);
        }

        public IHtmlString ValidationSummary(string message, bool excludeFieldErrors, object htmlAttributes) {
            return ValidationSummary(message, excludeFieldErrors, ObjectToDictionary(htmlAttributes));
        }

        public IHtmlString ValidationSummary(string message, bool excludeFieldErrors, IDictionary<string, object> htmlAttributes) {
            return BuildValidationSummary(message, excludeFieldErrors, htmlAttributes);
        }

        private IHtmlString BuildValidationSummary(string message, bool? excludeFieldErrors, IDictionary<string, object> htmlAttributes) {
            IEnumerable<ModelState> modelStates = null;
            if (excludeFieldErrors.HasValue && excludeFieldErrors.Value) {
                // Review: Is there a better way to share the form field name between this and ModelStateDictionary?
                var formModelState = ModelState[ModelStateDictionary.FormFieldKey];
                if (formModelState != null) {
                    modelStates = new [] { formModelState };
                }
            }else {
                modelStates = from state in ModelState
                              where state.Value.Errors.Any()
                              select state.Value;
            }

            if ((modelStates == null) || (!modelStates.Any())) {
                return null;
            }
            else {
                TagBuilder tagBuilder = new TagBuilder("div");
                tagBuilder.MergeAttributes(htmlAttributes);
                tagBuilder.AddCssClass(ValidationSummaryClass);

                StringBuilder builder = new StringBuilder();
                if (message != null) {
                    builder.Append("<span>");
                    builder.Append(Encode(message));
                    builder.AppendLine("</span>");
                }
                builder.AppendLine("<ul>");
                foreach (var modelState in modelStates) {
                    foreach(var error in modelState.Errors) {
                        builder.Append("<li>");
                        builder.Append(Encode(error));
                        builder.AppendLine("</li>");
                    }
                }
                builder.Append("</ul>");


                tagBuilder.InnerHtml = builder.ToString();
                return tagBuilder.ToHtmlString(TagRenderMode.Normal);
            }
        }
    }
}
