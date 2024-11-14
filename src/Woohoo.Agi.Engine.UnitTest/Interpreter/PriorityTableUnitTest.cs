// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.UnitTest.Interpreter;

using Woohoo.Agi.Engine.Interpreter;

public class PriorityTableUnitTest
{
    [Fact]
    public void CreateDefault()
    {
        var table = new PriorityTable();

        Console.WriteLine("Default priority table");
        for (int i = 0; i < 168; i++)
        {
            Console.WriteLine("Y = {0} ; Priority = {1}", i, table.GetPriorityAt(i));
        }

        table.GetPriorityAt(0).Should().Be(4);
        table.GetPriorityAt(47).Should().Be(4);
        table.GetPriorityAt(48).Should().Be(5);
        table.GetPriorityAt(59).Should().Be(5);
        table.GetPriorityAt(60).Should().Be(6);
        table.GetPriorityAt(71).Should().Be(6);
        table.GetPriorityAt(72).Should().Be(7);
        table.GetPriorityAt(83).Should().Be(7);
        table.GetPriorityAt(84).Should().Be(8);
        table.GetPriorityAt(95).Should().Be(8);
        table.GetPriorityAt(96).Should().Be(9);
        table.GetPriorityAt(107).Should().Be(9);
        table.GetPriorityAt(108).Should().Be(10);
        table.GetPriorityAt(119).Should().Be(10);
        table.GetPriorityAt(120).Should().Be(11);
        table.GetPriorityAt(131).Should().Be(11);
        table.GetPriorityAt(132).Should().Be(12);
        table.GetPriorityAt(143).Should().Be(12);
        table.GetPriorityAt(144).Should().Be(13);
        table.GetPriorityAt(155).Should().Be(13);
        table.GetPriorityAt(156).Should().Be(14);
        table.GetPriorityAt(167).Should().Be(14);
    }

    [Fact]
    public void SetBasePriority()
    {
        var table = new PriorityTable();
        table.SetBasePriority(6);

        Console.WriteLine("Priority table with base priority = 6");
        for (int i = 0; i < 168; i++)
        {
            Console.WriteLine("Y = {0} ; Priority = {1}", i, table.GetPriorityAt(i));
        }

        table.GetPriorityAt(0).Should().Be(4);
        table.GetPriorityAt(5).Should().Be(4);
        table.GetPriorityAt(6).Should().Be(5);
        table.GetPriorityAt(22).Should().Be(5);
        table.GetPriorityAt(23).Should().Be(6);
        table.GetPriorityAt(38).Should().Be(6);
        table.GetPriorityAt(39).Should().Be(7);
        table.GetPriorityAt(54).Should().Be(7);
        table.GetPriorityAt(55).Should().Be(8);
        table.GetPriorityAt(70).Should().Be(8);
        table.GetPriorityAt(71).Should().Be(9);
        table.GetPriorityAt(86).Should().Be(9);
        table.GetPriorityAt(87).Should().Be(10);
        table.GetPriorityAt(103).Should().Be(10);
        table.GetPriorityAt(104).Should().Be(11);
        table.GetPriorityAt(119).Should().Be(11);
        table.GetPriorityAt(120).Should().Be(12);
        table.GetPriorityAt(135).Should().Be(12);
        table.GetPriorityAt(136).Should().Be(13);
        table.GetPriorityAt(151).Should().Be(13);
        table.GetPriorityAt(152).Should().Be(14);
        table.GetPriorityAt(167).Should().Be(14);
    }
}
