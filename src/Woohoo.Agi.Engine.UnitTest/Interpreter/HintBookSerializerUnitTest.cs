// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.UnitTest.Interpreter;

using Woohoo.Agi.Engine.Interpreter.Hints;

public class HintBookSerializerUnitTest
{
    [Fact]
    public void Deserialize()
    {
        // Arrange
        var data = @"+topic1
-message1
- message2
-  message3 

+ topic 2
@ 10
- message 4
+ topic 3
@ 10,11
- message 5
~ gHrGtGs iqrGr
";

        // Act
        var actual = HintBookSerializer.Deserialize(new StringReader(data));

        // Assert
        actual.Should().BeEquivalentTo(new HintBook
        {
            Topics =
            [
                new Topic
                {
                    Title = "topic1",
                    Messages =
                    [
                        "message1",
                        "message2",
                        "message3",
                    ],
                },
                new Topic
                {
                    Title = "topic 2",
                    Messages =
                    [
                        "message 4",
                    ],
                    Rooms =
                    [
                        10,
                    ],
                },
                new Topic
                {
                    Title = "topic 3",
                    Messages =
                    [
                        "message 5",
                        "Opening Scene",
                    ],
                    Rooms =
                    [
                        10,
                        11,
                    ],
                },
            ],
        });
    }
}
