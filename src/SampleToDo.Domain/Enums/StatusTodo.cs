using System.ComponentModel;

namespace SampleToDo.Domain.Enums;

public enum StatusTodo
{
    [Description("Success")] Success = 0,
    [Description("Under review")] UnderReview = 1,
    [Description("Checked")] Checked = 2
}