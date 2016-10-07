using System.Collections.Generic;
using System.Globalization;



namespace System.Web.WebPages.Html {
    public partial class HtmlHelper {
        // Values from mvc
        private const int TextAreaRows = 2;
        private const int TextAreaColumns = 20;
        private static readonly IDictionary<string, object> implicitRowsAndColumns = new Dictionary<string, object> {
            { "rows", TextAreaRows.ToString(CultureInfo.InvariantCulture) },
            { "cols", TextAreaColumns.ToString(CultureInfo.InvariantCulture) },
        };

        private static IDictionary<string, object> GetRowsAndColumnsDictionary(int rows, int columns) {
            Dictionary<string, object> result = new Dictionary<string, object>();
            if (rows > 0) {
                result.Add("rows", rows.ToString(CultureInfo.InvariantCulture));
            }
            if (columns > 0) {
                result.Add("cols", columns.ToString(CultureInfo.InvariantCulture));
            }
            return result;
        }

        public IHtmlString TextArea(string name) {
            return TextArea(name,  value: null, htmlAttributes: (IDictionary<string, object>)null);
        }

        public IHtmlString TextArea(string name, object htmlAttributes) {
            return TextArea(name, value: null, htmlAttributes: ObjectToDictionary(htmlAttributes));
        }

        public IHtmlString TextArea(string name, IDictionary<string, object> htmlAttributes) {
            return TextArea(name, value: null, htmlAttributes: htmlAttributes);
        }

        public IHtmlString TextArea(string name, string value) {
            return TextArea(name, value, (IDictionary<string, object>)null);
        }

        public IHtmlString TextArea(string name, string value, object htmlAttributes) {
            return TextArea(name, value, ObjectToDictionary(htmlAttributes));
        }

        public IHtmlString TextArea(string name, string value, IDictionary<string, object> htmlAttributes) {
            if (String.IsNullOrEmpty(name)) {
                throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, "name");
            }

            return BuildTextArea(name, value, implicitRowsAndColumns, htmlAttributes);
        }

        public IHtmlString TextArea(string name, string value, int rows, int columns,
                object htmlAttributes) {
            return TextArea(name, value, rows, columns, ObjectToDictionary(htmlAttributes));
        }

        public IHtmlString TextArea(string name, string value, int rows, int columns,
                IDictionary<string, object> htmlAttributes) {
            if (String.IsNullOrEmpty(name)) {
                throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, "name");
            }
            return BuildTextArea(name, value, GetRowsAndColumnsDictionary(rows, columns), htmlAttributes);
        }

        private IHtmlString BuildTextArea(string name, string value, IDictionary<string, object> rowsAndColumnsDictionary,
                IDictionary<string, object> htmlAttributes) {

            TagBuilder tagBuilder = new TagBuilder("textarea");
            // Add user specified htmlAttributes
            tagBuilder.MergeAttributes(htmlAttributes);

            tagBuilder.MergeAttributes(rowsAndColumnsDictionary, rowsAndColumnsDictionary != implicitRowsAndColumns);

            // Value becomes the inner html of the textarea element
            var modelState = ModelState[name];
            if (modelState != null) {
                value = value ?? Convert.ToString(ModelState[name].Value, CultureInfo.CurrentCulture);
            }
            tagBuilder.InnerHtml = Encode(value);

            //Assign name and id
            tagBuilder.MergeAttribute("name", name);
            tagBuilder.GenerateId(name);

            AddErrorClass(tagBuilder, name);

            return tagBuilder.ToHtmlString(TagRenderMode.Normal);
        }
    }
}
