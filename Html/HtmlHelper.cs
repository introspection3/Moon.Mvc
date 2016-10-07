using System.Globalization;


namespace System.Web.WebPages.Html {
    public partial class HtmlHelper {
        private const string DefaultErrorClass = "field-validation-error";
        private const string DefaultValidationSummaryClass = "validation-summary-errors";
        private static readonly object _validationInputCssClassKey = new object();
        private static readonly object _validationSummaryClassKey = new object();
        private static string _idAttributeDotReplacement;

        internal HtmlHelper(ModelStateDictionary modelState) {
            ModelState = modelState;
        }

        // This property got copied from MVC's HtmlHelper along with TagBuilder.
        // It was a global property in MVC so it should not have scoped semantics here either.
        public static string IdAttributeDotReplacement {
            get {
                if (String.IsNullOrEmpty(_idAttributeDotReplacement)) {
                    _idAttributeDotReplacement = "_";
                }
                return _idAttributeDotReplacement;
            }
            set {
                _idAttributeDotReplacement = value;
            }
        }

        public static string ValidationInputCssClassName {
            get {
                return ScopeStorage.CurrentScope[_validationInputCssClassKey] as string ?? DefaultErrorClass;
            }
            set {
                if (value == null) {
                    throw new ArgumentNullException("value");
                }
                ScopeStorage.CurrentScope[_validationInputCssClassKey] = value;
            }
        }

        public static string ValidationSummaryClass {
            get {
                return ScopeStorage.CurrentScope[_validationSummaryClassKey] as string ?? DefaultValidationSummaryClass;
            }
            set {
                if (value == null) {
                    throw new ArgumentNullException("value");
                }
                ScopeStorage.CurrentScope[_validationSummaryClassKey] = value;
            }
        }

        private ModelStateDictionary ModelState {
            get;
            set;
        }


        public string AttributeEncode(object value) {
            return AttributeEncode(Convert.ToString(value, CultureInfo.InvariantCulture));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
            Justification = "For consistency, all helpers are instance methods.")]
        public string AttributeEncode(string value) {
            if (string.IsNullOrEmpty(value)) {
                return string.Empty;
            }
            else {
                return HttpUtility.HtmlAttributeEncode(value);
            }
        }

        public string Encode(object value) {
            return Encode(Convert.ToString(value, CultureInfo.InvariantCulture));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
            Justification = "For consistency, all helpers are instance methods.")]
        public string Encode(string value) {
            if (string.IsNullOrEmpty(value)) {
                return string.Empty;
            }
            else {
                return HttpUtility.HtmlEncode(value);
            }
        }

        /// <summary>
        /// Wraps HTML markup in an IHtmlString, which will enable HTML markup to be
        /// rendered to the output without getting HTML encoded.
        /// </summary>
        /// <param name="value">HTML markup string.</param>
        /// <returns>An IHtmlString that represents HTML markup.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
            Justification = "For consistency, all helpers are instance methods.")]
        public IHtmlString Raw(string value) {
            return new HtmlString(value);
        }
    }
}
