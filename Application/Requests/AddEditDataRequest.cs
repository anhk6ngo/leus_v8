namespace LeUs.Application.Requests;

public class AddEditDataRequest<T> where T: class
{
    public T? Data { get; set; }
    [Description("Action: ActionCommandType.Add=0, ActionCommandType.Edit=1")]
    public ActionCommandType Action { get; set; } = ActionCommandType.Add;
}