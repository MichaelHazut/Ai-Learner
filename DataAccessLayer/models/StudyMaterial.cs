using System.Collections;
using System.Reflection;

namespace DataAccessLayer.Models
{

    //StudyMaterial class is not an entity,
    //it is a model class that represents the study material that will be used to create a new study material in the database
    public class StudyMaterial
    {
        public required string Topic { get; set; } = "";
        public string Content { get; set; } = "Placeholder";
        public required string Summary { get; set; } = "";
        public required List<Questions> Questions { get; set; }

        public bool ValidateStudyMaterial()
        {
            Type type = typeof(StudyMaterial);
            PropertyInfo[] properties = type.GetProperties();

            foreach (var variable in properties)
            {
                //Check if variable is not a List<>
                if (!variable.PropertyType.IsGenericType)
                {
                    //Return false if is null
                    var value = variable.GetValue(this);
                    if (value == null || (string)value == string.Empty) return false;
                    continue;
                }

                //sending the List<> variable for validate it
                if (ValidateQuestions(variable) is not true) return false;
            }
            return true;
        }

        private bool ValidateQuestions(PropertyInfo variable)
        {
            //Get IEnumerable object from generic property
            IEnumerable? questions = variable.GetValue(this) as IEnumerable;

            //Iterate casting to an innerProps variable
            foreach (Questions question in questions!)
            {
                //Get Properties for each question
                PropertyInfo[] innerProps = question.GetType().GetProperties();

                foreach (var property in innerProps)
                {
                    //Check if variable is not a Dictionary<>
                    if (!property.PropertyType.IsGenericType)
                    {
                        var value = property.GetValue(question);
                        if (value == null || (string)value == string.Empty) return false;
                        continue;
                    }

                    //Get the Dictionary and validate it
                    var optionsDict = property.GetValue(question) as Dictionary<string, string>;
                    if (ValidateOptions(optionsDict) is not true) return false;
                }

            }
            return true;
        }

        private static bool ValidateOptions(Dictionary<string, string>? optionsDict)
        {
            if (optionsDict == null) return false;
            foreach (var keyValuePair in optionsDict!)
            {
                if (keyValuePair.Key == null || keyValuePair.Key == string.Empty) return false;
                if (keyValuePair.Value == null || keyValuePair.Value == string.Empty) return false;
            }
            return true;
        }
    }
}
