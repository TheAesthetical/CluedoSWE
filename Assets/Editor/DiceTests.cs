using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DiceTests
{

    [Test]
    public void Roll_ReturnsValueBetweenOneAndSix()
    {
        Dice dice = new Dice();

        for (int i = 0; i < 100; i++)
        {
            int result = dice.Roll();

            Assert.GreaterOrEqual(result, 1);
            Assert.LessOrEqual(result, 6);
        }
    }

}
