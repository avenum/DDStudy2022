namespace Api.Models.Push
{
    public class PushModel
    {
        public class AlertModel
        {
            public string? Title { get; set; }
            public string? Subtitle { get; set; }
            public string? Body { get; set; }
        }
        /// <summary>
        /// Счетчик у иконки приложения
        /// </summary>
        public int? Badge { get; set; }

        public string? Sound { get; set; }

        public AlertModel Alert { get; set; } = null!;

        public Dictionary<string, object> CustomData { get; set; } = null!;

    }

    public class SendPushModel
    {
        public Guid? UserId { get; set; }
        public PushModel Push { get; set; } = null!;
    }
}
