// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter
{
    /// <summary>
    /// Kernal API used by logic scripts.
    /// </summary>
    public interface IKernel
    {
        /// <summary>
        /// Returns true if the center of the base line of the object is inside
        /// the rectangle specified as its top left and bottom right corners.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        /// <param name="numericX1">Left position.</param>
        /// <param name="numericY1">Top position.</param>
        /// <param name="numericX2">Right position.</param>
        /// <param name="numericY2">Bottom position.</param>
        /// <returns>Returns <c>true</c> if the view object is inside the rectangle.</returns>
        bool CenterPosN(byte viewNumber, byte numericX1, byte numericY1, byte numericX2, byte numericY2);

        /// <summary>
        /// Returns true if the strings are identical (case insensitive comparison).
        /// </summary>
        /// <param name="stringA">First string object index.</param>
        /// <param name="stringB">Second string object index.</param>
        /// <returns>Returns <c>true</c> if the strings are identical.</returns>
        bool CompareStrings(byte stringA, byte stringB);

        /// <summary>
        /// Returns true if the event with the controller has occurred.  This
        /// happens if the key is pressed or the menu item is selected.
        /// </summary>
        /// <param name="controllerNumber">Controller number.</param>
        /// <returns>Returns <c>true</c> if the event has occurred.</returns>
        bool Controller(byte controllerNumber);

        /// <summary>
        /// Returns true if the value contained in variable A is equal to B.
        /// </summary>
        /// <param name="variableA">Variable A.</param>
        /// <param name="numericB">Immediate integer value.</param>
        /// <returns>Returns <c>true</c> if the values are equal.</returns>
        bool EqualN(byte variableA, byte numericB);

        /// <summary>
        /// Returns true if the value contained in variable A is equal to the
        /// value contained in variable B.
        /// </summary>
        /// <param name="variableA">Variable A.</param>
        /// <param name="variableB">Variable B.</param>
        /// <returns>Returns <c>true</c> if the values are equal.</returns>
        bool EqualV(byte variableA, byte variableB);

        /// <summary>
        /// Returns true if the value contained in variable A is greater than B.
        /// </summary>
        /// <param name="variableA">Variable A.</param>
        /// <param name="numericB">Immediate integer value.</param>
        /// <returns>Returns <c>true</c> if greater.</returns>
        bool GreaterN(byte variableA, byte numericB);

        /// <summary>
        /// Returns true if the value contained in variable A is greater than
        /// the value contained in variable B.
        /// </summary>
        /// <param name="variableA">Variable A.</param>
        /// <param name="variableB">Variable B.</param>
        /// <returns>Returns <c>true</c> if greater.</returns>
        bool GreaterV(byte variableA, byte variableB);

        /// <summary>
        /// Returns true if the room for the inventory item is 255 (belongs to
        /// the player).
        /// </summary>
        /// <param name="inventoryNumber">Inventory object index.</param>
        /// <returns>Returns <c>true</c> if item is in inventory.</returns>
        bool Has(byte inventoryNumber);

        /// <summary>
        /// Returns true if the user has pressed any key on the keyboard.  Used
        /// to create cycles to wait until any key is pressed.
        /// </summary>
        /// <returns>Returns <c>true</c> if a key has been pressed.</returns>
        bool HaveKey();

        /// <summary>
        /// Returns true if the flag is set.
        /// </summary>
        /// <param name="flagNumber">Flag index.</param>
        /// <returns>Returns <c>true</c> if flag is set.</returns>
        bool IsSet(byte flagNumber);

        /// <summary>
        /// Returns true if the flag specified in the variable is set.
        /// </summary>
        /// <param name="variableFlagNumber">Variable containing a flag index.</param>
        /// <returns>Returns <c>true</c> if flag is set.</returns>
        bool IsSetV(byte variableFlagNumber);

        /// <summary>
        /// Returns true if the value contained in variable
        /// <paramref name="variableA"/> is less than
        /// <paramref name="numericB"/>.
        /// </summary>
        /// <param name="variableA">Variable A.</param>
        /// <param name="numericB">Numeric value to compare.</param>
        /// <returns>Returns <c>true</c> if less.</returns>
        bool LessN(byte variableA, byte numericB);

        /// <summary>
        /// Returns true if the value contained in variable
        /// <paramref name="variableA"/> is less than the
        /// value contained in variable <paramref name="variableB"/>.
        /// </summary>
        /// <param name="variableA">Variable A.</param>
        /// <param name="variableB">Variable B.</param>
        /// <returns>Returns <c>true</c> if less.</returns>
        bool LessV(byte variableA, byte variableB);

        /// <summary>
        /// Returns true if the base of the object is completely within the
        /// rectangle specified using its top left and bottom right corners.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        /// <param name="numericX1">Left (0-159).</param>
        /// <param name="numericY1">Top (0-167).</param>
        /// <param name="numericX2">Right (0-159).</param>
        /// <param name="numericY2">Bottom (0-167).</param>
        /// <returns>Returns <c>true</c> if view object is completely within box.</returns>
        bool ObjInBox(byte viewNumber, byte numericX1, byte numericY1, byte numericX2, byte numericY2);

        /// <summary>
        /// Returns true if the room for the inventory item is the value
        /// specified in the variable.
        /// </summary>
        /// <param name="inventoryNumber">Inventory object index.</param>
        /// <param name="variableRoom">Room variable index.</param>
        /// <returns>Returns <c>true</c> if item is in room.</returns>
        bool ObjInRoom(byte inventoryNumber, byte variableRoom);

        /// <summary>
        /// Returns true if the coordinates of the base point of the cel which is
        /// the current image of the object are within the rectangle specified
        /// using its top left and bottom right corners.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        /// <param name="numericX1">Left (0-159).</param>
        /// <param name="numericY1">Top (0-167).</param>
        /// <param name="numericX2">Right (0-159).</param>
        /// <param name="numericY2">Bottom (0-167).</param>
        /// <returns>Returns <c>true</c> if view object base point is within rectangle.</returns>
        bool PosN(byte viewNumber, byte numericX1, byte numericY1, byte numericX2, byte numericY2);

        /// <summary>
        /// Returns true if the right side of the base line of the object is
        /// inside the rectangle specified as its top left and bottom right corners.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        /// <param name="numericX1">Left (0-159).</param>
        /// <param name="numericY1">Top (0-167).</param>
        /// <param name="numericX2">Right (0-159).</param>
        /// <param name="numericY2">Bottom (0-167).</param>
        /// <returns>Returns <c>true</c> if view object right side is within rectangle.</returns>
        bool RightPosN(byte viewNumber, byte numericX1, byte numericY1, byte numericX2, byte numericY2);

        /// <summary>
        /// Returns true if the parse results match the word identifiers passed in.
        /// </summary>
        /// <param name="wordIds">Family word identifiers.</param>
        /// <returns>Returns <c>true</c> if parse results match the words.</returns>
        bool Said(int[] wordIds);

        /// <summary>
        /// Enables input from the player. The player will now be able to enter
        /// game input.
        /// </summary>
        void AcceptInput();

        /// <summary>
        /// Performs an addition.
        /// </summary>
        /// <remarks>
        /// No checking takes place, so if the result is greater than 255, it
        /// will wrap around to zero and above.
        /// </remarks>
        /// <param name="variableA">Source and destination variable index.</param>
        /// <param name="numericB">Immediate integer value.</param>
        void AddN(byte variableA, byte numericB);

        /// <summary>
        /// Adds a view to the picture as if it were orignally part of the
        /// picture including priority and control.
        /// </summary>
        /// <remarks>
        /// The specified View is drawn onto to the background picture with the
        /// specified Loop and Cel at the given coordinates, X and Y with Pri as
        /// it's priority. Because of the priority, it can be drawn behind other
        /// parts of the picture making it seamless. The Margin parameter is the
        /// priority control number for the bottom control box. It can be between
        /// 0 and 3. If the value is above 3, it will not have a control base.
        /// The control box will be a minimum of one pixel high, and as high as
        /// the view or the next priority line (which ever is shorter).
        /// </remarks>
        /// <param name="numericViewResourceIndex">View resource.</param>
        /// <param name="numericLoop">Loop.</param>
        /// <param name="numericCel">Cel.</param>
        /// <param name="numericX">X coordinate.</param>
        /// <param name="numericY">Y coordinate.</param>
        /// <param name="numericPriority">View priority.</param>
        /// <param name="numericMargin">Margin.</param>
        void AddToPicture(byte numericViewResourceIndex, byte numericLoop, byte numericCel, byte numericX, byte numericY, byte numericPriority, byte numericMargin);

        /// <summary>
        /// Adds a view to the picture as if it were orignally part of the
        /// picture including priority and control.
        /// </summary>
        /// <remarks>
        /// The view number specified in variable vView is drawn onto to the
        /// background picture with the specified vLoop and vCel at the given
        /// coordinates, vX and vY with vPri as it's priority. Because of the
        /// priority, it can be drawn behind other parts of the picture making
        /// it seamless. The vMargin parameter is the priority control number
        /// for the bottom control box. It can be between 0 and 3. If the value
        /// is above 3, it will not have a control base. The control box will be
        /// a minimum of one pixel high, and as high as the view or the next
        /// priority line (which ever is shorter).
        /// </remarks>
        /// <param name="variableViewResourceIndex">View resource variable index.</param>
        /// <param name="variableLoop">Loop variable index.</param>
        /// <param name="variableCel">Cel variable index.</param>
        /// <param name="variableX">X coordinate variable index.</param>
        /// <param name="variableY">Y coordinate variable index.</param>
        /// <param name="variablePriority">Priority variable index.</param>
        /// <param name="variableMargin">Margin variable index.</param>
        void AddToPictureV(byte variableViewResourceIndex, byte variableLoop, byte variableCel, byte variableX, byte variableY, byte variablePriority, byte variableMargin);

        /// <summary>
        /// Performs an addition.
        /// </summary>
        /// <remarks>
        /// No checking takes place, so if the result is greater than 255, it
        /// will wrap around to zero and above.
        /// </remarks>
        /// <param name="variableA">Source and destination variable index.</param>
        /// <param name="variableB">Value variable index.</param>
        void AddV(byte variableA, byte variableB);

        /// <summary>
        /// Toggles the movement of the ego. Only used in the last versions of
        /// AGI version 3.
        /// </summary>
        void AdjEgoMoveToXY();

        /// <summary>
        /// Enables or disables the menu bar.
        /// </summary>
        /// <param name="numericEnabled">
        /// Zero to disable the menu bar, non-zero to enable.
        /// </param>
        void AllowMenu(byte numericEnabled);

        /// <summary>
        /// Sets the view object to be animated, if it is not already.
        /// </summary>
        /// <remarks>
        /// Sets the view object motion to <see cref="Motion.Normal"/>, its
        /// cycle to <see cref="CycleMode.Normal"/> and its direction to
        /// <see cref="Direction.Motionless"/>.
        /// </remarks>
        /// <param name="viewNumber">View object index.</param>
        void AnimateObj(byte viewNumber);

        /// <summary>
        /// Assigns a variable.
        /// </summary>
        /// <param name="variableA">Destination variable index.</param>
        /// <param name="numericB">Value to assign.</param>
        void AssignN(byte variableA, byte numericB);

        /// <summary>
        /// Assigns a variable.
        /// </summary>
        /// <param name="variableA">Destination variable index.</param>
        /// <param name="variableB">Source variable index.</param>
        void AssignV(byte variableA, byte variableB);

        /// <summary>
        /// Turns on view object blocking and sets the block to the region
        /// specified by the X1, Y1, X2 and Y2 coordinates. Objects that observe
        /// blocks will be constrained to this area.
        /// </summary>
        /// <param name="numericX1">Left (0-159).</param>
        /// <param name="numericY1">Top (0-167).</param>
        /// <param name="numericX2">Right (0-159).</param>
        /// <param name="numericY2">Bottom (0-167).</param>
        void Block(byte numericX1, byte numericY1, byte numericX2, byte numericY2);

        /// <summary>
        /// The logic specified by num is executed following the call command.
        /// When the logic returns, execution of the previous logic which called
        /// it resumes.
        /// </summary>
        /// <param name="numericLogicResourceIndex">Logic resource index.</param>
        void Call(byte numericLogicResourceIndex);

        /// <summary>
        /// The logic specified by variable vNum is executed following the call
        /// command. When the logic returns, execution of the previous logic
        /// which called it resumes.
        /// </summary>
        /// <param name="variableLogicResourceIndex">Logic resource index.</param>
        void CallV(byte variableLogicResourceIndex);

        /// <summary>
        /// Clears any text currently entered on the input line.
        /// </summary>
        void CancelLine();

        /// <summary>
        /// Clears the specified rows of text to the specified color.
        /// </summary>
        /// <param name="numericRowTop">Top row (each row is 8 pixels high).</param>
        /// <param name="numericRowBottom">Bottom row.</param>
        /// <param name="numericColor">Zero for black, non-zero for white.</param>
        void ClearLines(byte numericRowTop, byte numericRowBottom, byte numericColor);

        /// <summary>
        /// Clears the specified text region to the specified color.
        /// </summary>
        /// <param name="numericRowTop">Top row (each row is 8 pixels high).</param>
        /// <param name="numericColumnTop">Left column.</param>
        /// <param name="numericRowBottom">Bottom row.</param>
        /// <param name="numericColumnBottom">Right column.</param>
        /// <param name="numericColor">Zero for black, non-zero for white.</param>
        void ClearTextRectangle(byte numericRowTop, byte numericColumnTop, byte numericRowBottom, byte numericColumnBottom, byte numericColor);

        /// <summary>
        /// Closes the currently displayed message box, but does not erase it,
        /// and thus, when a new message box is drawn, it could be drawn over
        /// the old one.
        /// </summary>
        void CloseDialogue();

        /// <summary>
        /// Closes and erases the currently displayed message box from the screen.
        /// </summary>
        void CloseWindow();

        /// <summary>
        /// Sets up the screen's elements to specified coordinates.
        /// </summary>
        /// <param name="numericPlayTop">
        /// Top margin for the graphical area, in text coordinates.
        /// </param>
        /// <param name="numericInputLine">
        /// Position of the input line, in text coordinates.
        /// </param>
        /// <param name="numericStatusLine">
        /// Position of the status bar, in text coordinates.
        /// </param>
        void ConfigureScreen(byte numericPlayTop, byte numericInputLine, byte numericStatusLine);

        /// <summary>
        /// Stores the view object's current cel in the specified variable.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        /// <param name="variableCelNumber">Destination variable index.</param>
        void CurrentCel(byte viewNumber, byte variableCelNumber);

        /// <summary>
        /// Stores the view object's current loop in the specified variable.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        /// <param name="variableLoopNumber">Destination variable index.</param>
        void CurrentLoop(byte viewNumber, byte variableLoopNumber);

        /// <summary>
        /// Stores the view object's current view in the specified variable.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        /// <param name="variableViewNumber">Destination variable index.</param>
        void CurrentView(byte viewNumber, byte variableViewNumber);

        /// <summary>
        /// Sets the view object's cycle time.
        /// </summary>
        /// <remarks>
        /// The cycle time specifies how frequently to update the view object's
        /// animation cycle, that is how often to move to the next frame. This
        /// time is measured in interpreter cycles. If the cycle time is set to
        /// zero, the view object will not cycle at all.
        /// </remarks>
        /// <param name="viewNumber">View object index.</param>
        /// <param name="variableTime">Variable index.</param>
        void CycleTime(byte viewNumber, byte variableTime);

        /// <summary>
        /// Decrements a variable.
        /// </summary>
        /// <remarks>
        /// If the value is 0, it will not be decremented and remain 0,
        /// preventing it from rolling over back to 255.
        /// </remarks>
        /// <param name="variableA">Variable index.</param>
        void Decrement(byte variableA);

        /// <summary>
        /// Disables all menu items which have the specified controller
        /// assigned to them.
        /// </summary>
        /// <param name="controllerNumber">Controller number.</param>
        void DisableItem(byte controllerNumber);

        /// <summary>
        /// Unloads the specified picture resource from memory.
        /// </summary>
        /// <param name="variablePictureResourceIndex">Resource index.</param>
        void DiscardPicture(byte variablePictureResourceIndex);

        /// <summary>
        /// Unloads the specified sound resource from memory.
        /// </summary>
        /// <param name="numericSoundResourceIndex">Resource index.</param>
        void DiscardSound(byte numericSoundResourceIndex);

        /// <summary>
        /// Unloads the specified view resource from memory.
        /// </summary>
        /// <param name="numericViewResourceIndex">Resource index.</param>
        void DiscardView(byte numericViewResourceIndex);

        /// <summary>
        /// Unloads the specified view resource from memory.
        /// </summary>
        /// <param name="variableViewResourceIndex">Resource index.</param>
        void DiscardViewV(byte variableViewResourceIndex);

        /// <summary>
        /// Displays a string of text on the screen using the current text
        /// color attributes.
        /// </summary>
        /// <param name="numericRow">Row, in text coordinates.</param>
        /// <param name="numericColumn">Column, in text coordinates.</param>
        /// <param name="messageNumber">Message index.</param>
        void Display(byte numericRow, byte numericColumn, byte messageNumber);

        /// <summary>
        /// Displays a string of text on the screen using the current text
        /// color attributes.
        /// </summary>
        /// <param name="numericRow">Row, in text coordinates.</param>
        /// <param name="numericColumn">Column, in text coordinates.</param>
        /// <param name="variableMessageNumber">Message variable index.</param>
        void DisplayV(byte numericRow, byte numericColumn, byte variableMessageNumber);

        /// <summary>
        /// Stores the distance between two view objects in the specified
        /// variable.
        /// </summary>
        /// <remarks>
        /// If either of the view objects is not visible, the distance is set
        /// to 255. Otherwise, it is set to the absolute distance, which will
        /// be a maximum of 254.
        /// </remarks>
        /// <param name="viewNumberA">First view object index.</param>
        /// <param name="viewNumberB">Second view object index.</param>
        /// <param name="variableDistance">Destination variable index.</param>
        void Distance(byte viewNumberA, byte viewNumberB, byte variableDistance);

        /// <summary>
        /// Performs a division.
        /// </summary>
        /// <param name="variableA">Source and destination variable index.</param>
        /// <param name="numericB">Divisor integer value.</param>
        void DivN(byte variableA, byte numericB);

        /// <summary>
        /// Performs a division.
        /// variable.
        /// </summary>
        /// <param name="variableA">Source and destination variable index.</param>
        /// <param name="variableB">Divisor variable index.</param>
        void DivV(byte variableA, byte variableB);

        /// <summary>
        /// Draws the view object specified by voNum onto the screen. It
        /// solidifies its positioning on screen, erases all the animated
        /// objects from the screen, then redraws them all with the new object.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        void Draw(byte viewNumber);

        /// <summary>
        /// Clears the off screen buffer and draws the specified picture resource.
        /// </summary>
        /// <remarks>
        /// To update the picture on the screen, use <see cref="ShowPicture"/>.
        /// </remarks>
        /// <param name="variablePictureResourceIndex">
        /// Variable containing a picture resource index.
        /// </param>
        void DrawPicture(byte variablePictureResourceIndex);

        /// <summary>
        /// Removes an item from the player's inventory.
        /// inventory.
        /// </summary>
        /// <remarks>
        /// This is done by setting the item's current room to 0.
        /// </remarks>
        /// <param name="inventoryNumber">Inventory object index.</param>
        void Drop(byte inventoryNumber);

        /// <summary>
        /// Recalls the last line of input the player made allowing them to
        /// modify it and submit it again.
        /// </summary>
        void EchoLine();

        /// <summary>
        /// Enables all menu items which have the specified controller
        /// assigned to them.
        /// </summary>
        /// <param name="controllerNumber">Controller number.</param>
        void EnableItem(byte controllerNumber);

        /// <summary>
        /// Sets the view object's cycle mode to
        /// <see cref="CycleMode.NormalEnd"/>. It will cycle normally from
        /// beginning to end.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        /// <param name="flagNotify">
        /// Flag index. It is first reset, then set when the last cel of the
        /// loop is reached.
        /// </param>
        void EndOfLoop(byte viewNumber, byte flagNotify);

        /// <summary>
        /// Erases the view object from the screen. It erases all the animated
        /// objects from the screen, and if the view object is flagged for
        /// updating, the unanimated objects as well, then redraws them without
        /// the erased object.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        void Erase(byte viewNumber);

        /// <summary>
        /// In the versions of AGI which had mouse support, the Amiga, Atari ST,
        /// Macintosh and Apple IIgs versions, this function set up an invisible
        /// barrier in which the mouse cursor was restricted to.
        /// </summary>
        /// <param name="numericX1">Left (0-159).</param>
        /// <param name="numericY1">Top (0-167).</param>
        /// <param name="numericX2">Right (0-159).</param>
        /// <param name="numericY2">Bottom (0-167).</param>
        void FenceMouse(byte numericX1, byte numericY1, byte numericX2, byte numericY2);

        /// <summary>
        /// Sets the view object's loop to a fixed value, rather than
        /// automatically updating based on the current direction.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        void FixLoop(byte viewNumber);

        /// <summary>
        /// Resets the flag <paramref name="flagDone"/>, then the view object
        /// <paramref name="viewNumber"/> will be set up to move from its
        /// current coordinate to the new coordinate to the ego's (view object 0)
        /// position. If <paramref name="numericStepSize"/> is nonzero, the view
        /// object's follow step size will be set to its value, otherwise it
        /// will be set to the view object's step size. Upon the view object
        /// reaching the destination, the flag <paramref name="flagDone"/>
        /// will be set.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        /// <param name="numericStepSize">Step size.</param>
        /// <param name="flagDone">Flag index.</param>
        void FollowEgo(byte viewNumber, byte numericStepSize, byte flagDone);

        /// <summary>
        /// Immediately erases all the current view objects on screen, redraws
        /// them, then updates them. The parameter is unused in the majority of
        /// interpreters. It was only used in the very early versions when they
        /// only updated the specified view object.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        void ForceUpdate(byte viewNumber);

        /// <summary>
        /// Adds the item to the player's inventory.
        /// This is done by setting the item's current room to 255.
        /// </summary>
        /// <param name="inventoryNumber">Inventory object index.</param>
        void Get(byte inventoryNumber);

        /// <summary>
        /// Stores the view object's direction in the specified variable.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        /// <param name="variableDirection">Destination variable index.</param>
        void GetDir(byte viewNumber, byte variableDirection);

        /// <summary>
        /// Prompts the player to enter a number with
        /// <paramref name="messageCaption"/> as the request text.
        /// The result is stored in variable <paramref name="variableNumber"/>.
        /// </summary>
        /// <param name="messageCaption">Caption message index.</param>
        /// <param name="variableNumber">Destination variable index.</param>
        void GetNumber(byte messageCaption, byte variableNumber);

        /// <summary>
        /// Stores the view object's position in the specified variables.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        /// <param name="variableX">Variable index to hold X coordinate.</param>
        /// <param name="variableY">Variable index to hold Y coordinate.</param>
        void GetPosition(byte viewNumber, byte variableX, byte variableY);

        /// <summary>
        /// Stores the view object's current priority in the specified variable.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        /// <param name="variablePriority">Destination variable index.</param>
        void GetPriority(byte viewNumber, byte variablePriority);

        /// <summary>
        /// Stores the inventory item's room number in the specified variable.
        /// </summary>
        /// <param name="inventoryNumber">Inventory object index.</param>
        /// <param name="variableRoomNumber">Destination variable index.</param>
        void GetRoomV(byte inventoryNumber, byte variableRoomNumber);

        /// <summary>
        /// Retrieves a string of input from the player.
        /// Places a message and input line at <paramref name="numericRow"/>
        /// and <paramref name="numericColumn"/>.
        /// The string entered will be limited to <paramref name="numericMaxLength"/>,
        /// and then stored in <paramref name="stringDestination"/>.
        /// </summary>
        /// <param name="stringDestination">Destination string object index.</param>
        /// <param name="messageCaption">Caption message index.</param>
        /// <param name="numericRow">Row index (0-24).</param>
        /// <param name="numericColumn">Column index.</param>
        /// <param name="numericMaxLength">Max string length.</param>
        void GetString(byte stringDestination, byte messageCaption, byte numericRow, byte numericColumn, byte numericMaxLength);

        /// <summary>
        /// Adds the item to the player's inventory.
        /// This is done by setting the item's current room to 255.
        /// </summary>
        /// <param name="variableInventoryNumber">Variable index.</param>
        void GetV(byte variableInventoryNumber);

        /// <summary>
        /// Places the game back into graphics mode. The screen is redrawn and
        /// now menus, message boxes, views or other graphics will be able to
        /// be used.
        /// </summary>
        void Graphics();

        /// <summary>
        /// Hides the mouse cursor.
        /// </summary>
        void HideMouse();

        /// <summary>
        /// Sets the interpreter in a mode where the player must hold down the
        /// arrow key or joystick to move the ego. Once they release the button,
        /// the ego will stop moving. In normal mode, the player would press it
        /// once to move the ego, and again to stop the ego.
        /// </summary>
        void HoldKey();

        /// <summary>
        /// Allows the view object to ignore object blocking and move freely.
        /// It also allows the view object to walk past control screen boundaries.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        void IgnoreBlocks(byte viewNumber);

        /// <summary>
        /// Allows the view object to ignore the horizon.
        /// It will be allowed to move past the top horizon line normally the
        /// boundary for moving objects.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        void IgnoreHorizon(byte viewNumber);

        /// <summary>
        /// Allows the view object to ignore other objects.
        /// When the collision check is made, no check between this and
        /// another view object will return true. Both view objects need to have
        /// their observe objs flag set or they will be unable to collide.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        void IgnoreObjects(byte viewNumber);

        /// <summary>
        /// Increments a variable.
        /// </summary>
        /// <remarks>
        /// If value is 255, it will not be incremented and remain 255,
        /// preventing it from rolling over back to zero.
        /// </remarks>
        /// <param name="variableA">Variable index.</param>
        void Increment(byte variableA);

        /// <summary>
        /// An obsolete command from the original days of AGI on the AppleII and
        /// other older versions. It was used to allows the user to format
        /// (initialize) the floppy disk for saved games. It is not implemented in
        /// newer versions of AGI.
        /// </summary>
        void InitDisk();

        /// <summary>
        /// Calibrates the joystick if one is connected to the computer.
        /// </summary>
        void InitJoy();

        /// <summary>
        /// Stores the view object's current loop's last cel in the specified
        /// variable.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        /// <param name="variableCelNumber">Destination variable index.</param>
        void LastCel(byte viewNumber, byte variableCelNumber);

        /// <summary>
        /// Stores an immediate integer value in the variable specified by the
        /// value in the specified variable.
        /// </summary>
        /// <param name="variableA">
        /// Index of variable containing the destination variable index.
        /// </param>
        /// <param name="numericB">Immediate integer value to store.</param>
        void LIndirectN(byte variableA, byte numericB);

        /// <summary>
        /// Copies the value of a variable in the variable specified by the
        /// value in the specified variable.
        /// </summary>
        /// <param name="variableA">
        /// Index of variable containing the destination variable index.
        /// </param>
        /// <param name="variableB">Index of variable to copy.</param>
        void LIndirectV(byte variableA, byte variableB);

        /// <summary>
        /// Loads the logic resource from the volume file into RAM so it could
        /// be executed.
        /// </summary>
        /// <param name="numericLogicResourceIndex">Resource index.</param>
        void LoadLogics(byte numericLogicResourceIndex);

        /// <summary>
        /// Loads the logic resource from the volume file into RAM so it could
        /// be executed.
        /// </summary>
        /// <param name="variableLogicResourceIndex">Variable index.</param>
        void LoadLogicsV(byte variableLogicResourceIndex);

        /// <summary>
        /// Loads the picture resource from the volume file into RAM so it
        /// could be used.
        /// </summary>
        /// <param name="variablePictureResourceIndex">Variable index.</param>
        void LoadPicture(byte variablePictureResourceIndex);

        /// <summary>
        /// Loads the sound resource from the volume file into RAM so it could
        /// be used.
        /// </summary>
        /// <param name="numericSoundResourceIndex">Resource index.</param>
        void LoadSound(byte numericSoundResourceIndex);

        /// <summary>
        /// Loads the view resource from the volume file into RAM so it could
        /// be used.
        /// </summary>
        /// <param name="numericViewResourceIndex">Resource index.</param>
        void LoadView(byte numericViewResourceIndex);

        /// <summary>
        /// Loads the view resource from the volume file into RAM so it could
        /// be used.
        /// </summary>
        /// <param name="variableViewResourceIndex">Variable index.</param>
        void LoadViewV(byte variableViewResourceIndex);

        /// <summary>
        /// Logs current game information to the file named 'logfile'.
        /// </summary>
        /// <remarks>
        /// Information logged such as the current room number, player input,
        /// and the specified message.
        /// </remarks>
        /// <param name="messageNote">Message index.</param>
        void Log(byte messageNote);

        /// <summary>
        /// Activates the menu bar for item selection.
        /// </summary>
        /// <remarks>
        /// If fMenuInput (f14) is set, it will not be activated. Once
        /// activated, if the player selects an item, its controller will be
        /// set.
        /// </remarks>
        void MenuInput();

        /// <summary>
        /// Stores the current coordinates of the mouse in the specified
        /// variables.
        /// </summary>
        /// <param name="variableX">Variable index to hold X coordinate.</param>
        /// <param name="variableY">Variable index to hold Y coordinate.</param>
        void MousePosN(byte variableX, byte variableY);

        /// <summary>
        /// Resets the flag <paramref name="flagDone"/>, then the view object
        /// specified by <paramref name="viewNumber"/> will be set up to move
        /// from its current coordinate to the new coordinate specified by the
        /// <paramref name="numericX"/> and <paramref name="numericY"/>
        /// parameters. If the <paramref name="numericStepSize"/> is
        /// nonzero, the view object's step size will be set to its value.
        /// Upon the view object reaching the destination, the flag
        /// <paramref name="flagDone"/> will be set.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        /// <param name="numericX">X position (0-159).</param>
        /// <param name="numericY">Y position (0-167).</param>
        /// <param name="numericStepSize">Step size.</param>
        /// <param name="flagDone">Flag index.</param>
        void MoveObj(byte viewNumber, byte numericX, byte numericY, byte numericStepSize, byte flagDone);

        /// <summary>
        /// Resets the flag <paramref name="flagDone"/>, then the view object
        /// specified by <paramref name="viewNumber"/> will be set up to move
        /// from its current coordinate to the new coordinate specified by the
        /// <paramref name="variableX"/> and <paramref name="variableY"/>
        /// parameters. If the variable <paramref name="variableStepSize"/>
        /// value is nonzero, the view object's step size will be
        /// set to its value. Upon the view object reaching the destination,
        /// the flag <paramref name="flagDone"/> will be set.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        /// <param name="variableX">X position variable index.</param>
        /// <param name="variableY">Y position variable index.</param>
        /// <param name="variableStepSize">Step size variable index.</param>
        /// <param name="flagDone">Flag index.</param>
        void MoveObjV(byte viewNumber, byte variableX, byte variableY, byte variableStepSize, byte flagDone);

        /// <summary>
        /// The variable <paramref name="variableA"/> will be multiplied by the
        /// value of <paramref name="numericB"/>, an immediate integer value.
        /// No checking takes place, so if the result is greater than 255, it
        /// will wrap around to zero and above.
        /// </summary>
        /// <param name="variableA">Variable A.</param>
        /// <param name="numericB">Immediate integer value.</param>
        void MulN(byte variableA, byte numericB);

        /// <summary>
        /// The variable <paramref name="variableA"/> will be multiplied by the
        /// value of <paramref name="variableB"/>, a variable.
        /// No checking takes place, so if the result is greater than 255, it
        /// will wrap around to zero and above.
        /// </summary>
        /// <param name="variableA">Variable A.</param>
        /// <param name="variableB">Variable B.</param>
        void MulV(byte variableA, byte variableB);

        /// <summary>
        /// Sets the current room.
        /// </summary>
        /// <remarks>
        /// Each interpreter cycle, Logic.000 is executed, which then calls the
        /// current room's logic.
        /// </remarks>
        /// <param name="numericRoomNumber">Room number (logic resource index).</param>
        void NewRoom(byte numericRoomNumber);

        /// <summary>
        /// Sets the current room.
        /// </summary>
        /// <remarks>
        /// Each interpreter cycle, Logic.000 is executed, which then calls the
        /// current room's logic.
        /// </remarks>
        /// <param name="variableRoomNumber">
        /// Room number (logic resource index) variable index.
        /// </param>
        void NewRoomV(byte variableRoomNumber);

        /// <summary>
        /// Sets the cycle mode of the view object to
        /// <see cref="CycleMode.Normal"/>.
        /// The view object will animate looping the cels from first to last.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        void NormalCycle(byte viewNumber);

        /// <summary>
        /// Sets the motion of the view object to <see cref="Motion.Normal"/>.
        /// </summary>
        /// <remarks>
        /// It will stop any special movement. If the view object was following,
        /// wandering or whatnot, it will now resume normal movement.
        /// </remarks>
        /// <param name="viewNumber">View object index.</param>
        void NormalMotion(byte viewNumber);

        /// <summary>
        /// Stores the view object's current view number of loops into the
        /// specified variable.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        /// <param name="variableTotalLoops">Variable to hold number of loops.</param>
        void NumberOfLoops(byte viewNumber, byte variableTotalLoops);

        /// <summary>
        /// Allows the view object to move around on any type of priority
        /// region including water as long as it does not bump into the control
        /// boundary.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        void ObjectOnAnything(byte viewNumber);

        /// <summary>
        /// Sets the view object to be constrained to only move around in areas
        /// of the screen which do not completely have the water control bit
        /// set, which are any pixels on the priority screen not with the value
        /// of 3 (other than other control boundaries).
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        void ObjectOnLand(byte viewNumber);

        /// <summary>
        /// Sets the view object to be constrained to only move around in areas
        /// of the screen which have the water control bit set (any pixels on
        /// the priority screen with the value of 3).
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        void ObjectOnWater(byte viewNumber);

        /// <summary>
        /// Displays information regarding the view object.
        /// It displays the view object number, coordinates, size, priority and
        /// step size.
        /// </summary>
        /// <param name="variableViewObjectNumber">Variable index.</param>
        void ObjectStatusV(byte variableViewObjectNumber);

        /// <summary>
        /// Forces the view object to be constrained to the block region, if
        /// the view object blocking parameter has been set up.
        /// It also forces the view object to be unable to walk past control
        /// screen boundaries.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        void ObserveBlocks(byte viewNumber);

        /// <summary>
        /// Sets the view object to react to the horizon.
        /// It will not be allowed to move past the top horizon line.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        void ObserveHorizon(byte viewNumber);

        /// <summary>
        /// Sets the view object to observe other objects.
        /// When the collision check is made, it will be checked between this
        /// and another view object. If either view object is not set to
        /// observe other view objects, they will be unable to collide.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        void ObserveObjects(byte viewNumber);

        /// <summary>
        /// If <see cref="CloseDialogue"/> has been called and a new message
        /// box is displayed, the previous one will not be cleared. However,
        /// if <see cref="CloseDialogue"/> is called, it will close the message
        /// box and refresh the screen before drawing the new one.
        /// </summary>
        void OpenDialogue();

        /// <summary>
        /// Draws the specified picture resource without clearing the picture
        /// buffer prior to drawing.
        /// </summary>
        /// <remarks>
        /// Because only white areas are filled, any fills the overlaid picture
        /// makes on non-white areas will be ignored.
        /// </remarks>
        /// <param name="variablePictureResourceIndex">Variable index.</param>
        void OverlayPicture(byte variablePictureResourceIndex);

        /// <summary>
        /// Parses the specified string as it would the normal line of
        /// input by the player.
        /// </summary>
        /// <remarks>
        /// If the parse results in success, it sets
        /// <see cref="Flags.PlayerCommandLine"/>. It is commonly used to
        /// verify that the string entered is valid.
        /// </remarks>
        /// <param name="stringInput">Index of string object to parse.</param>
        void Parse(byte stringInput);

        /// <summary>
        /// Pauses the game by displaying the message box and waits for
        /// the player to press a button or key to close it.
        /// </summary>
        void Pause();

        /// <summary>
        /// Gives control of the ego back to the player back and sets the view
        /// object's motion back to normal. The player will now be able to
        /// control and move the ego.
        /// </summary>
        void PlayerControl();

        /// <summary>
        /// Polls the mouse and stores the button state to v27, the X
        /// coordinate to v28 and X coordinate to v29.
        /// This is a custom extension to AGI.
        /// </summary>
        void PollMouse();

        /// <summary>
        /// Sets the current script position to the position saved by
        /// <see cref="PushScript"/>, discarding any elements added to
        /// the script after <see cref="PushScript"/> was been called.
        /// </summary>
        void PopScript();

        /// <summary>
        /// Sets the view object's position to the specified coordinates.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        /// <param name="numericX">X coordinate (0-159).</param>
        /// <param name="numericY">Y coordinate (0-167).</param>
        void Position(byte viewNumber, byte numericX, byte numericY);

        /// <summary>
        /// Sets the view object's position to the specified coordinates.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        /// <param name="variableX">X coordinate variable index.</param>
        /// <param name="variableY">Y coordinate variable index.</param>
        void PositionV(byte viewNumber, byte variableX, byte variableY);

        /// <summary>
        /// Disables input from the player. The player will not be able to enter
        /// input until <see cref="AcceptInput"/> is called.
        /// </summary>
        void PreventInput();

        /// <summary>
        /// Displays the message box on the screen.
        /// </summary>
        /// <remarks>
        /// <see cref="Flags.PrintMode"/> controls whether or not to automatically
        /// close the message box. If set, the box will remain until
        /// <see cref="CloseWindow"/> is called, and the game execution will
        /// continue with the message box on the screen.
        /// If <see cref="Flags.PrintMode"/> is not set,
        /// <see cref="Variables.WindowTimer"/> (unit of 1/2 second) is used to
        /// control the timing of the message box. If zero, the message box will
        /// remain on the screen until the player presses a button.
        /// The maximum message width is 30.
        /// </remarks>
        /// <param name="messageNumber">Message index.</param>
        void Print(byte messageNumber);

        /// <summary>
        /// Displays the message box on the screen at the specified coordinates.
        /// </summary>
        /// <param name="messageText">Message index.</param>
        /// <param name="numericRow">Row.</param>
        /// <param name="numericColumn">Column.</param>
        /// <param name="numericWidth">Maximum width of message.</param>
        void PrintAt(byte messageText, byte numericRow, byte numericColumn, byte numericWidth);

        /// <summary>
        /// Displays the message box on the screen at the specified coordinates.
        /// </summary>
        /// <param name="variableText">Message variable index.</param>
        /// <param name="numericRow">Row.</param>
        /// <param name="numericColumn">Column.</param>
        /// <param name="numericWidth">Maximum width of message.</param>
        void PrintAtV(byte variableText, byte numericRow, byte numericColumn, byte numericWidth);

        /// <summary>
        /// Displays the message box on the screen.
        /// </summary>
        /// <remarks>
        /// <see cref="Flags.PrintMode"/> controls whether or not to automatically
        /// close the message box. If set, the box will remain until
        /// <see cref="CloseWindow"/> is called, and the game execution will
        /// continue with the message box on the screen.
        /// If <see cref="Flags.PrintMode"/> is not set,
        /// <see cref="Variables.WindowTimer"/> (unit of 1/2 second) is used to
        /// control the timing of the message box. If zero, the message box will
        /// remain on the screen until the player presses a button.
        /// The maximum message width is 30.
        /// </remarks>
        /// <param name="variableMessageNumber">Message variable index.</param>
        void PrintV(byte variableMessageNumber);

        /// <summary>
        /// Releases control of the ego from the player back to the computer.
        /// </summary>
        /// <remarks>
        /// The player will no longer be able to control the ego, only the logic
        /// can manipulate the view object.
        /// Call <see cref="PlayerControl"/> to give control back to the player.
        /// </remarks>
        void ProgramControl();

        /// <summary>
        /// Saves the current position in the game script for later retrieval
        /// using <see cref="PopScript"/>.
        /// </summary>
        /// <remarks>
        /// It does not actually push any value onto the stack though, so if
        /// you use it twice, the previous saved value will be lost.
        /// </remarks>
        void PushScript();

        /// <summary>
        /// Sets the inventory item current room. If it was in the player's
        /// inventory before, it no longer will be.
        /// </summary>
        /// <param name="inventoryNumber">Inventory object index.</param>
        /// <param name="numericRoomNumber">Room number.</param>
        void Put(byte inventoryNumber, byte numericRoomNumber);

        /// <summary>
        /// Sets the inventory item current room. If it was in the player's
        /// inventory before, it no longer will be.
        /// </summary>
        /// <param name="inventoryNumber">Inventory object index.</param>
        /// <param name="variableRoomNumber">Room number variable index.</param>
        void PutV(byte inventoryNumber, byte variableRoomNumber);

        /// <summary>
        /// Exits the game.
        /// </summary>
        /// <remarks>
        /// If <paramref name="numericImmediate"/> is nonzero, it quits immediately.
        /// Otherwise, it prompts the user. If the player presses Enter it will
        /// quit. If they press ESC it will not.
        /// </remarks>
        /// <param name="numericImmediate">Zero to prompt, otherwise quit immediately.</param>
        void Quit(byte numericImmediate);

        /// <summary>
        /// Generates and stores a random number into the specified variable.
        /// </summary>
        /// <param name="numericStart">Lower boundary.</param>
        /// <param name="numericEnd">Upper boundary.</param>
        /// <param name="variableDestination">Destination variable index.</param>
        void Random(byte numericStart, byte numericEnd, byte variableDestination);

        /// <summary>
        /// Sets the interpreter back to normal ego movement mode.
        /// In this mode, the player presses the directional key
        /// or button once to move the ego, and again to stop the ego.
        /// </summary>
        void ReleaseKey();

        /// <summary>
        /// Sets the view object's current loop to be automatically set by the
        /// interpreter based on its current direction.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        void ReleaseLoop(byte viewNumber);

        /// <summary>
        /// Sets the view object's priority value to be automatically set based
        /// on the current Y coordinate of the view object.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        void ReleasePriority(byte viewNumber);

        /// <summary>
        /// Erases and repositions the view object using relative coordinates.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        /// <param name="variableX">Relative X coordinate (-128 to +127).</param>
        /// <param name="variableY">Relative Y coordinate (-128 to +127).</param>
        void Reposition(byte viewNumber, byte variableX, byte variableY);

        /// <summary>
        /// Erases and repositions the view object using absolute coordinates.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        /// <param name="numericX">Absolute X coordinate.</param>
        /// <param name="numericY">Absolute Y coordinate.</param>
        void RepositionTo(byte viewNumber, byte numericX, byte numericY);

        /// <summary>
        /// Erases and repositions the view object using absolute coordinates.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        /// <param name="variableX">Absolute X coordinate variable index.</param>
        /// <param name="variableY">Absolute Y coordinate variable index.</param>
        void RepositionToV(byte viewNumber, byte variableX, byte variableY);

        /// <summary>
        /// Resets a flag.
        /// </summary>
        /// <param name="flagA">Flag index.</param>
        void Reset(byte flagA);

        /// <summary>
        /// Clears the bookmark in the current logic. When the logic is called
        /// again, it will begin execution from the beginning of the logic as it
        /// would normally.
        /// </summary>
        void ResetScanStart();

        /// <summary>
        /// Resets a flag.
        /// </summary>
        /// <param name="variableA">Flag variable index.</param>
        void ResetV(byte variableA);

        /// <summary>
        /// Restarts the game, discarding any current progress in the game. It
        /// restarts the interpreter then sets the fRestart flag (f06) and begins
        /// execution of the game from logic.000. The game's logics then process
        /// the flag accordingly, generally using it to know whether to start in
        /// the first room or from the title screen, whether to set up the menu,
        /// assign the controller keys, etc.
        /// </summary>
        void RestartGame();

        /// <summary>
        /// Executes the game selection dialog and resumes the selected saved game.
        /// </summary>
        void RestoreGame();

        /// <summary>
        /// Returns from the currently executed logic back to either the previous
        /// logic which called it, or in the case of logic.000, back to the interpreter.
        /// </summary>
        void ReturnFalse();

        /// <summary>
        /// Sets the view object's cycle mode to <see cref="CycleMode.Reverse"/>.
        /// The view object will animate looping the cels from last to first.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        void ReverseCycle(byte viewNumber);

        /// <summary>
        /// Sets the view object's cycle mode to <see cref="CycleMode.ReverseEnd"/>.
        /// The view object will animate looping the cels from last to first.
        /// When the first cel of the loop is reached, the specified flag will
        /// be set.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        /// <param name="flagNotify">Index of flag to set when loop is done.</param>
        void ReverseLoop(byte viewNumber, byte flagNotify);

        /// <summary>
        /// Sets the value of <paramref name="variableA"/> to the value contained
        /// in the variable whose index is specified in <paramref name="variableB"/>.
        /// </summary>
        /// <param name="variableA">Variable A.</param>
        /// <param name="variableB">Variable B.</param>
        void RIndirect(byte variableA, byte variableB);

        /// <summary>
        /// Saves the current game in progress to a file so it can be restored
        /// (resumed) at a later time. It executes a save dialog for the user to
        /// select a game and/or (re)name it. It then saves all the information
        /// needed to a file.
        /// </summary>
        /// <remarks>
        /// It saves the current variables, flags, strings, inventory information,
        /// and significant interpreter variables. It also saves the "script",
        /// which contains the information regarding which resources have been
        /// loaded, which pictures and picviews have been drawn, etc.
        /// </remarks>
        void SaveGame();

        /// <summary>
        /// Sets the game's script size to the value specified.
        /// The script was used by save games to store information regarding
        /// which resources were loaded, pictures were drawn, picviews were drawn, etc.
        /// </summary>
        /// <param name="numericSize">Script size.</param>
        void ScriptSize(byte numericSize);

        /// <summary>
        /// Sets a flag.
        /// </summary>
        /// <param name="flagA">Flag index.</param>
        void Set(byte flagA);

        /// <summary>
        /// Sets the view object's current cel.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        /// <param name="numericCelNumber">Cel number.</param>
        void SetCel(byte viewNumber, byte numericCelNumber);

        /// <summary>
        /// Sets the view object's current cel.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        /// <param name="variableCelNumber">Cel number variable index.</param>
        void SetCelV(byte viewNumber, byte variableCelNumber);

        /// <summary>
        /// Sets the current cursor character to the first character in the
        /// message string. The cursor character is the character which
        /// trails the line of input on the PC.
        /// </summary>
        /// <param name="messageCharacter">Message index.</param>
        void SetCursorChar(byte messageCharacter);

        /// <summary>
        /// Sets the view object's direction.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        /// <param name="variableDirection">
        /// Direction variable index, where the variable value is between 0 and 8.
        /// </param>
        void SetDir(byte viewNumber, byte variableDirection);

        /// <summary>
        /// Sets the game ID of the game data.
        /// </summary>
        /// <remarks>
        /// Each interpreter contained an embedded ID such as "KQ1". Sierra's games
        /// would then run this command with the game id such as "set.game.id("KQ1");"
        /// and in initialization logic. If the message string did not match the
        /// interpreter's game id, it would exit. This was likely used to prevent
        /// games from being run on an incorrect version of interpreter.
        /// </remarks>
        /// <param name="messageId">Message index.</param>
        void SetGameId(byte messageId);

        /// <summary>
        /// Sets the horizon boundary line to the specified Y coordinate.
        /// </summary>
        /// <remarks>
        /// Objects observing the horizon will not be allowed to move past this
        /// line. The default coordinate is 36 and it is reset to the default
        /// on each call to <see cref="NewRoom(byte)"/> or
        /// <see cref="NewRoomV(byte)"/>.
        /// </remarks>
        /// <param name="numericY">Horizon Y coordinate.</param>
        void SetHorizon(byte numericY);

        /// <summary>
        /// Sets the controller to be activated by the specified key code.
        /// </summary>
        /// <remarks>
        /// After this is called, when the player presses the set key, the
        /// controller will be set.
        /// </remarks>
        /// <param name="numericCode">Key code.</param>
        /// <param name="controllerNumber">Controller number.</param>
        void SetKey(int numericCode, byte controllerNumber);

        /// <summary>
        /// Sets the view object's current loop.
        /// </summary>
        /// <remarks>
        /// If the current cel number is out of the new loop's range, the
        /// current cel number is then set to zero.
        /// </remarks>
        /// <param name="viewNumber">View object index.</param>
        /// <param name="numericLoopNumber">Loop number.</param>
        void SetLoop(byte viewNumber, byte numericLoopNumber);

        /// <summary>
        /// Sets the view object's current loop.
        /// </summary>
        /// <remarks>
        /// If the current cel number is out of the new loop's range, the
        /// current cel number is then set to zero.
        /// </remarks>
        /// <param name="viewNumber">View object index.</param>
        /// <param name="variableLoopNumber">Loop number variable index.</param>
        void SetLoopV(byte viewNumber, byte variableLoopNumber);

        /// <summary>
        /// Adds a menu to the menu bar for placing items under with the
        /// specified message as its caption. All items added following
        /// will be added under this menu.
        /// </summary>
        /// <param name="messageText">Message index.</param>
        void SetMenu(byte messageText);

        /// <summary>
        /// Adds a menu item to the current menu with the specified message
        /// as its caption.
        /// The new menu controller is set, and every time the player
        /// selects the item, the controller will be set.
        /// </summary>
        /// <param name="messageText">Message index.</param>
        /// <param name="controllerNumber">Controller number.</param>
        void SetMenuItem(byte messageText, byte controllerNumber);

        /// <summary>
        /// Sets the new priority base.
        /// </summary>
        /// <remarks>
        /// Normally the first 48 rows of pixels are priority 4, and the
        /// following are 5, 6, 7, incrementing the priority every 12 rows.
        /// This allows the player to have more priorities in a smaller region.
        /// </remarks>
        /// <param name="numericBase">Base priority.</param>
        void SetPriBase(byte numericBase);

        /// <summary>
        /// Sets the view object's priority.
        /// </summary>
        /// <remarks>
        /// Normally the priority is set automatically based on the Y coordinate
        /// of the view object. However, with this command, you can set it to be
        /// fixed on any priority value regardless of coordinate.
        /// The priority value can be anywhere from 0 to 15 (though controls are
        /// 0-3 and priorities are 4-15). If it is 15, the object will be drawn
        /// above everything and ignore all boundaries specified by control lines.
        /// </remarks>
        /// <param name="viewNumber">View object index.</param>
        /// <param name="numericPriority">Priority (0-15).</param>
        void SetPriority(byte viewNumber, byte numericPriority);

        /// <summary>
        /// Sets the view object's priority.
        /// </summary>
        /// <remarks>
        /// Normally the priority is set automatically based on the Y coordinate
        /// of the view object. However, with this command, you can set it to be
        /// fixed on any priority value regardless of coordinate.
        /// The priority value can be anywhere from 0 to 15 (though controls are
        /// 0-3 and priorities are 4-15). If it is 15, the object will be drawn
        /// above everything and ignore all boundaries specified by control lines.
        /// </remarks>
        /// <param name="viewNumber">View object index.</param>
        /// <param name="variablePriority">Priority variable index.</param>
        void SetPriorityV(byte viewNumber, byte variablePriority);

        /// <summary>
        /// Bookmarks the current position in the current logic for later
        /// execution. When the logic is called again, it will begin execution
        /// from just after this command was called rather than the beginning of
        /// the logic.
        /// </summary>
        void SetScanStart();

        /// <summary>
        /// Sets the save game mode to automatic by giving a predefined name
        /// for a save game.
        /// </summary>
        /// <param name="stringAutoSave">Save game name string object index.</param>
        void SetSimple(byte stringAutoSave);

        /// <summary>
        /// Copies a message to a string object.
        /// </summary>
        /// <remarks>
        /// If the message is longer than 40 bytes, only the first 40 bytes or
        /// copied.
        /// </remarks>
        /// <param name="stringDestination">Destination string object index.</param>
        /// <param name="messageSource">Source message index.</param>
        void SetString(byte stringDestination, byte messageSource);

        /// <summary>
        /// Sets the text colors.
        /// </summary>
        /// <param name="numericForeground">Foreground, can be any color (0-15).</param>
        /// <param name="numericBackground">Background, must be either black (zero) or white (non-zero).</param>
        void SetTextAttribute(byte numericForeground, byte numericBackground);

        /// <summary>
        /// An obsolete command from the early versions of AGI. It does nothing
        /// in the later versions.
        /// </summary>
        /// <param name="numericTop">Top.</param>
        /// <param name="numericLeft">Left.</param>
        void SetUpperLeft(byte numericTop, byte numericLeft);

        /// <summary>
        /// Sets the specified flag.
        /// </summary>
        /// <param name="variableA">Flag variable index.</param>
        void SetV(byte variableA);

        /// <summary>
        /// Sets the view object to the specified view resource.
        /// </summary>
        /// <remarks>
        /// The view object can then be drawn with the cels from the new view
        /// resource. If the current loop number it out of the new view's loop
        /// range, the current loop is then set to zero, as with the cels.
        /// </remarks>
        /// <param name="viewNumber">View object index.</param>
        /// <param name="numericViewResourceIndex">View resource index.</param>
        void SetView(byte viewNumber, byte numericViewResourceIndex);

        /// <summary>
        /// Sets the view object to the specified view resource.
        /// </summary>
        /// <remarks>
        /// The view object can then be drawn with the cels from the new view
        /// resource. If the current loop number it out of the new view's loop
        /// range, the current loop is then set to zero, as with the cels.
        /// </remarks>
        /// <param name="viewNumber">View object index.</param>
        /// <param name="variableViewResourceIndex">View resource variable index.</param>
        void SetViewV(byte viewNumber, byte variableViewResourceIndex);

        /// <summary>
        /// Shakes the screen num amount of times then continues execution of
        /// the game. This is used for earthquake scenes and the like.
        /// </summary>
        /// <param name="numericShakeCount">Number of shakes.</param>
        void ShakeScreen(byte numericShakeCount);

        /// <summary>
        /// Displays the current memory information such as how much memory is
        /// being used, the maximum that has been used, etc.
        /// </summary>
        void ShowMemory();

        /// <summary>
        /// In the versions of AGI which had mouse support, the Amiga, Atari ST,
        /// Macintosh and Apple IIgs versions, this function made the mouse
        /// cursor visible if it were previously hidden.
        /// </summary>
        void ShowMouse();

        /// <summary>
        /// Displays a view resource on the screen along with its embedded
        /// description in a message box.
        /// It is generally used in conjunction with <see cref="Status"/> and
        /// f13 to display the inventory items.
        /// </summary>
        /// <param name="numericViewResourceIndex">View resource index.</param>
        void ShowObj(byte numericViewResourceIndex);

        /// <summary>
        /// Displays a view resource on the screen along with its embedded
        /// description in a message box.
        /// </summary>
        /// <remarks>
        /// It is generally used in conjunction with <see cref="Status"/> and
        /// <see cref="Flags.StatusSelect"/> to display the inventory items.
        /// </remarks>
        /// <param name="variableViewResourceIndex">View resource variable index.</param>
        void ShowObjV(byte variableViewResourceIndex);

        /// <summary>
        /// Draws the current off screen picture buffer on the screen,
        /// filling the whole graphical play area with a background.
        /// </summary>
        void ShowPicture();

        /// <summary>
        /// Draws the current picture buffer's priority picture on the screen
        /// to view until a key is pressed. It then redraws the screen as normal
        /// and resumes the game.
        /// </summary>
        void ShowPriScreen();

        /// <summary>
        /// Starts playing a sound.
        /// </summary>
        /// <remarks>
        /// The flag is reset when play starts, and is set when play has completed.
        /// </remarks>
        /// <param name="numericSoundResourceIndex">Resource index.</param>
        /// <param name="flagDone">Flag index.</param>
        void Sound(byte numericSoundResourceIndex, byte flagDone);

        /// <summary>
        /// Sets the view object to cycle. The cels will animate according to
        /// their cycle type.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        void StartCycling(byte viewNumber);

        /// <summary>
        /// Sets the view object's motion to <see cref="Motion.Normal"/>.
        /// </summary>
        /// <remarks>
        /// If the view object specified is the ego, the variable
        /// <see cref="Variables.Direction"/>) is updated and the player control
        /// is enabled.
        /// </remarks>
        /// <param name="viewNumber">View object index.</param>
        void StartMotion(byte viewNumber);

        /// <summary>
        /// If the view object specified by voNum is not flagged for updating,
        /// all view objects on screen are erased, view object specified is
        /// flagged to be updated, then the view objects are redrawn.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        void StartUpdate(byte viewNumber);

        /// <summary>
        /// Brings up the inventory item status screen, which is a text screen
        /// with the names of the objects.
        /// </summary>
        /// <remarks>
        /// If <see cref="Flags.StatusSelect"/> is set, it allows you to select
        /// an item to view it and its description. Otherwise it's simply a list
        /// of the items. If the items are selectable,
        /// <see cref="Variables.StatusSelectedItem"/> will be assigned the
        /// value, which will be used in conjunction with
        /// <see cref="ObjectStatusV"/> to display the object information.
        /// </remarks>
        void Status();

        /// <summary>
        /// Turns the status bar off making it invisible.
        /// </summary>
        void StatusLineOff();

        /// <summary>
        /// Turns the status bar on making it visible at its configured coordinate.
        /// </summary>
        void StatusLineOn();

        /// <summary>
        /// Sets the view object's step size.
        /// </summary>
        /// <remarks>
        /// The step size specifies how many pixels to move the view object per
        /// movement step.
        /// </remarks>
        /// <param name="viewNumber">View object index.</param>
        /// <param name="variableStepSize">Step size variable index.</param>
        void StepSize(byte viewNumber, byte variableStepSize);

        /// <summary>
        /// Sets the view object's step time.
        /// </summary>
        /// <remarks>
        /// The step time specifies how many interpreter cycles to wait before
        /// updating the view object's movement.
        /// </remarks>
        /// <param name="viewNumber">View object index.</param>
        /// <param name="variableStepTime">Step time.</param>
        void StepTime(byte viewNumber, byte variableStepTime);

        /// <summary>
        /// Stops cycling of the view object, meaning the cels will no longer
        /// be animating.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        void StopCycling(byte viewNumber);

        /// <summary>
        /// Stops movement of the view object.
        /// </summary>
        /// <remarks>
        /// Its direction is set to <see cref="Direction.Motionless"/>, and its
        /// motion back to <see cref="Motion.Normal"/>. If the view object
        /// specified is the ego, the variable vEgoDirection (v6) is updated and
        /// the player control is disabled.
        /// </remarks>
        /// <param name="viewNumber">View object index.</param>
        void StopMotion(byte viewNumber);

        /// <summary>
        /// If a sound is currently playing, it stops, and the sound flag is
        /// set to indicate the sound is done.
        /// </summary>
        void StopSound();

        /// <summary>
        /// If the view object specified by voNum is flagged for updating, all
        /// view objects on screen are erased, view object specified is unflagged
        /// to not be updated, then the view objects are redrawn.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        void StopUpdate(byte viewNumber);

        /// <summary>
        /// Finalizes the menu bar with it's menus and menu items. It will now be
        /// ready to be accessed through <see cref="MenuInput"/>.
        /// </summary>
        void SubmitMenu();

        /// <summary>
        /// The value of B, where B is an immediate integer value, is subtracted
        /// from variable vA. No checking takes place, so if the result is less
        /// than 0, it will wrap around to 255 and below.
        /// </summary>
        /// <param name="variableA">Variable A.</param>
        /// <param name="numericB">Immediate integer value.</param>
        void SubN(byte variableA, byte numericB);

        /// <summary>
        /// The value of vB, where vB is numericB variable, is subtracted from variable vA.
        /// No checking takes place, so if the result is less than 0, it will
        /// wrap around to 255 and below.
        /// </summary>
        /// <param name="variableA">Variable A.</param>
        /// <param name="variableB">Variable B.</param>
        void SubV(byte variableA, byte variableB);

        /// <summary>
        /// Places the game in text mode with numericB 40x25 character text screen.
        /// </summary>
        /// <remarks>
        /// The background is cleared to the text background color attribute, and
        /// the text written by default will be in the text foreground color
        /// attribute. It also resets the current text row and columns to 0,0.
        /// In text mode, no menus, message boxes, views or other graphics can
        /// be used.
        /// </remarks>
        void TextScreen();

        /// <summary>
        /// Toggles the value of a flag.
        /// </summary>
        /// <param name="flagA">Flag index.</param>
        void Toggle(byte flagA);

        /// <summary>
        /// Allows the player to toggle between different video modes.
        /// </summary>
        void ToggleMonitor();

        /// <summary>
        /// Toggles the value of a flag.
        /// </summary>
        /// <param name="variableA">Flag variable index.</param>
        void ToggleV(byte variableA);

        /// <summary>
        /// Configures the trace mode.
        /// </summary>
        /// <param name="numericLogicResourceIndex">
        /// Logic resource index which contains the logic command names stored as messages.
        /// </param>
        /// <param name="numericTop">Message box Y coordinate.</param>
        /// <param name="numericHeight">Message box height.</param>
        void TraceInfo(byte numericLogicResourceIndex, byte numericTop, byte numericHeight);

        /// <summary>
        /// Turns the trace mode on for debugging purposes. It will only work
        /// if fDebug (f10) is set. It allows the user to trace through the
        /// logic code as it is executed.
        /// </summary>
        void TraceOn();

        /// <summary>
        /// Deactivates and erases all the objects currently visible on the screen.
        /// </summary>
        void UnanimateAll();

        /// <summary>
        /// Turns off view object blocking. Objects will no longer be constrained
        /// to the block region, regardless of whether they observe blocks or not.
        /// </summary>
        void Unblock();

        /// <summary>
        /// Displays a message box with the interpreter's name and version information.
        /// </summary>
        void Version();

        /// <summary>
        /// Sets the view object's motion to <see cref="Motion.Wander"/> which
        /// causes the view object to wander aimlessly and randomly around the
        /// screen. If the view object specified is the ego (view object #0),
        /// the player control is disabled.
        /// </summary>
        /// <param name="viewNumber">View object index.</param>
        void Wander(byte viewNumber);

        /// <summary>
        /// Retrieves a word from the player input and places it in a string.
        /// </summary>
        /// <param name="stringDestination">String object index.</param>
        /// <param name="numericWordNumber">Word number in player input starting at 0.</param>
        void WordToString(byte stringDestination, byte numericWordNumber);
    }
}
