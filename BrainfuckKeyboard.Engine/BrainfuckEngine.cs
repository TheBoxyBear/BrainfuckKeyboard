using System.Text;

namespace BrainfuckKeyboard.Engine
{
    public class BrainfuckEngine
    {
        public event EventHandler<char>? OutputReceived;

        private readonly PointedCollection<byte> memory = new();
        private readonly Stack<int> loopStack = new();
        private readonly IList<char> tokenBuffer;
        private readonly Func<char>? getInput;
        private int tokenIndex, pendingLoops;

        public BrainfuckEngine(Func<char>? getInput = null, IList<char>? tokenBuffer = null)
        {
            this.getInput = getInput;
            this.tokenBuffer = tokenBuffer ?? new List<char>();
        }

        public static string Run(IList<char> tokens, Func<char>? getInput = null)
        {
            var builder = new StringBuilder();
            var engine = new BrainfuckEngine(getInput, tokens);

            engine.Execute(output => builder.Append(output));
            return builder.ToString();
        }

        public void AddToken(char token)
        {
            ValidateToken(token);

            tokenBuffer.Add(token);
            Execute(TriggerOutputEvent);
        }

        private void ValidateToken(char token)
        {
            //if (token is not '+' or '-' or '<' or '>' or '[' or ']' or ',' or '.')
            //    throw new InvalidOperationException($"'{token}' is not a valid token.");
        }

        private void Execute(Action<char> handleOutput)
        {
            while (tokenIndex < tokenBuffer.Count)
            {
                var token = tokenBuffer[tokenIndex];

                while (pendingLoops > 0)
                {
                    if (token == ']')
                        pendingLoops--;

                    if (tokenIndex++ == tokenBuffer.Count)
                        return;
                }

                switch (token)
                {
                    case '+':
                        unchecked { memory.PointedItem++; }
                        break;
                    case '-':
                        unchecked { memory.PointedItem--; }
                        break;
                    case '<':
                        memory.MoveBack();
                        break;
                    case '>':
                        memory.MoveForward();
                        break;
                    case '[':
                        if (memory.PointedItem == 0)
                            pendingLoops++;
                        loopStack.Push(tokenIndex);
                        break;
                    case ']':
                        if (memory.PointedItem != 0)
                            tokenIndex = loopStack.Peek();
                        else
                            loopStack.Pop();
                        break;
                    case ',':
                        if (getInput is null)
                            throw new InvalidOperationException("Engine instance does not support input.");

                        memory.PointedItem = (byte)getInput();
                        break;
                    case '.':
                        handleOutput((char)memory.PointedItem);
                        break;
                }

                tokenIndex++;
            }
        }

        private void TriggerOutputEvent(char output) => OutputReceived?.Invoke(this, output);
    }
}