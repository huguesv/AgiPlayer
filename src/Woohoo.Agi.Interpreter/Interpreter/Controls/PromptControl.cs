// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter.Controls
{
    public class PromptControl
    {
        public PromptControl(AgiInterpreter interpreter)
        {
            this.Interpreter = interpreter ?? throw new ArgumentNullException(nameof(interpreter));
            this.Text = string.Empty;
        }

        public string Text { get; set; }

        protected AgiInterpreter Interpreter { get; }

        protected WindowManager WindowManager => this.Interpreter.WindowManager;

        protected IInputDriver InputDriver => this.Interpreter.InputDriver;

        public bool DoModal()
        {
            this.WindowManager.DisplayMessageBox(this.Text, 0, 0, false);

            bool accepted = this.Interpreter.InputDriver.PollAcceptOrCancel(0);
            this.WindowManager.CloseWindow();

            return accepted;
        }
    }
}
