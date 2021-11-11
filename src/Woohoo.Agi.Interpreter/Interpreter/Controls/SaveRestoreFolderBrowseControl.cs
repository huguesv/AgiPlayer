// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter.Controls;

public class SaveRestoreFolderBrowseControl
{
    public SaveRestoreFolderBrowseControl(AgiInterpreter interpreter)
    {
        this.Interpreter = interpreter ?? throw new ArgumentNullException(nameof(interpreter));
        this.Title = string.Empty;
        this.FolderPath = string.Empty;
    }

    public string Title { get; set; }

    public string FolderPath { get; set; }

    protected AgiInterpreter Interpreter { get; }

    protected WindowManager WindowManager => this.Interpreter.WindowManager;

    protected IInputDriver InputDriver => this.Interpreter.InputDriver;

    public bool DoModal()
    {
        string error;

        do
        {
            var inputBox = new InputBoxControl(this.Interpreter)
            {
                Title = this.Title,
                Text = this.FolderPath,
            };

            if (inputBox.DoModal())
            {
                if (Directory.Exists(inputBox.Text))
                {
                    this.FolderPath = inputBox.Text;
                    return true;
                }

                error = string.Format(CultureInfo.CurrentCulture, UserInterface.SavePathDoesNotExistFormat, inputBox.Text);
            }
            else
            {
                this.FolderPath = string.Empty;
                return false;
            }
        }
        while (this.Interpreter.Prompt(error));

        this.FolderPath = string.Empty;
        return false;
    }
}
