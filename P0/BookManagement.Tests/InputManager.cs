public class InputManager {
    private readonly TextReader _originalInput;
    private readonly Queue<string> _inputs;

    public InputManager() {
        _originalInput = Console.In;
        _inputs = new Queue<string>();
    }

    public void SetInput(params string[] inputs) {
        foreach (var input in inputs) {
            _inputs.Enqueue(input);
        }
        var stringReader = new StringReader(string.Join(Environment.NewLine, _inputs));
        Console.SetIn(stringReader);
    }

    public void RestoreInput() {
        Console.SetIn(_originalInput);  
    }

    public string ReadLine() {
        return Console.ReadLine()!;
    }
}
