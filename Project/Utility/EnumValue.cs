using SinaExpoBot.Domain.Enum;
using System.Configuration;

namespace SinaExpoBot.Utility
{
    public static class EnumValue
    {
        public static string GetEnumValue(this CenterTypeEnum? type)
        {
            var result = "";
            if (type == CenterTypeEnum.Private)
                result = "غیردولتی";
            else if (type == CenterTypeEnum.Public)
                result = "دولتی";
            return result;
        }

        public static string GetEnumValue(this CenterGenderEnum? type)
        {
            var result = "";
            if (type == CenterGenderEnum.Both)
                result = "مختلط";
            else if (type == CenterGenderEnum.Female)
                result = "دخترانه";
            else if (type == CenterGenderEnum.Male)
                result = "پسرانه";

            return result;
        }

        public static string GetEnumValue(this MembersGradeEnum? type)
        {
            var result = "";
            if (type == MembersGradeEnum.Elementary1)
                result = "دوره اول دبستان";
            else if (type == MembersGradeEnum.Elementary2)
                result = "دوره دوم دبستان";
            else if (type == MembersGradeEnum.MiddleSchool1)
                result = "دوره اول متوسطه";
            else if (type == MembersGradeEnum.MiddleSchool2)
                result = "دوره دوم متوسطه";

            return result;
        }



        public static CenterGenderEnum? GetGenderEnumByValue(this string input)
        {
            var result = new CenterGenderEnum();
            if (input == "دخترانه")
                result = CenterGenderEnum.Female;
            else if (input == "پسرانه")
                result = CenterGenderEnum.Male;
            else if (input == "مختلط")
                result = CenterGenderEnum.Both;

            return result;
        }

        public static MembersGradeEnum? GetGradeEnumByValue(this string input)
        {
            var result = new MembersGradeEnum();
            if (input == "دوره اول دبستان")
                result = MembersGradeEnum.Elementary1;
            else if (input == "دوره دوم دبستان")
                result = MembersGradeEnum.Elementary2;
            else if (input == "دوره اول متوسطه")
                result = MembersGradeEnum.MiddleSchool1;
            else if (input == "دوره دوم متوسطه")
                result = MembersGradeEnum.MiddleSchool2;

            return result;
        }

        public static CenterTypeEnum? GetCenterTypeEnumByValue(this string input)
        {
            var result = new CenterTypeEnum();
            if (input == "دولتی")
                result = CenterTypeEnum.Public;
            else if (input == "غیردولتی")
                result = CenterTypeEnum.Private;
           

            return result;
        }
    }
}