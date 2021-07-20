namespace RecrutariBlazorWasm.Shared.Entities
{
    public class LanguageQualification
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public int QualificationId { get; set; }
        public int ApplicantId { get; set; }
        public string LanguageName { get; set; }
        public string QualificationName { get; set; }

        //public LanguageQualification(int id, int languageId, int qualificationId, int applicantId, string languageName, string qualificationName)
        //{
        //    this.Id = id;
        //    this.LanguageId = languageId;
        //    this.QualificationId = qualificationId;
        //    this.ApplicantId = applicantId;
        //    this.LanguageName = languageName;
        //    this.QualificationName = qualificationName;
        //}

        //public LanguageQualification(int languageId, int qualificationId)
        //{
        //    this.LanguageId = languageId;
        //    this.QualificationId = qualificationId;
        //}




        //public bool Equals(LanguageQualification other)
        //{
        //    return this.LanguageId.Equals(other.LanguageId) && this.QualificationId.Equals(other.QualificationId);
        //}

        //public static bool operator ==(LanguageQualification a, LanguageQualification b)
        //{
        //    return a.LanguageId.Equals(b.LanguageId) && a.QualificationId.Equals(b.QualificationId);
        //}

        //public static bool operator !=(LanguageQualification a, LanguageQualification b)
        //{
        //    return !(a == b);
        //}

        //public override bool Equals(object obj)
        //{
        //    return Equals(obj as LanguageQualification);
        //}

        //public override int GetHashCode()
        //{
        //    return HashCode.Combine(LanguageId, QualificationId);
        //}
    }
}
