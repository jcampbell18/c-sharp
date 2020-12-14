namespace campbelljproj2d
{
    /// <summary>
    /// represents the trivia table from the database
    /// </summary>
    class TriviaQuestion
    {
        public int QuestionId { get; set; }
        public string Question { get; set; }

        public string Choice1 { get; set; }

        public string Choice2 { get; set; }

        public string Choice3 { get; set; }

        public string Answer { get; set; }

        public string AnswerInfo { get; set; }

    }
}
