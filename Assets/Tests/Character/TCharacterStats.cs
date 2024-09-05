using System.Collections;
using System.Collections.Generic;
using Character.Base;
using Character.Class.Player;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.Character
{
    public class TCharacterStats
    {
        private CharacterStats _characterStats;

        private void ResetCharacterStats()
        {
            _characterStats = new CharacterStats(new Class001Player(), 1);
        }
    
        [Test]
        public void TestLevelUp()
        {
            ResetCharacterStats();
            _characterStats.LevelUp();
            Assert.AreEqual(2, _characterStats.Level);
        }
    
        [Test]
        public void TestRandomizeBuildPoints()
        {
            ResetCharacterStats();
            _characterStats = new CharacterStats(new Class001Player(), 1, true);
            Assert.GreaterOrEqual(_characterStats.BuildPoints, 5);
        }
    
        [Test]
        public void TestRandomizeBuildPointsWithLevel()
        {
            ResetCharacterStats();
            _characterStats = new CharacterStats(new Class001Player(), 10, true);
            Assert.GreaterOrEqual(_characterStats.BuildPoints, 50);
        }
    
        [Test]
        public void TestRandomizeBuildPointsWithLevelAndLevelUp()
        {
            ResetCharacterStats();
            _characterStats = new CharacterStats(new Class001Player(), 10, true);
            _characterStats.LevelUp();
            Assert.GreaterOrEqual(_characterStats.BuildPoints, 55);
        }

        [Test]
        public void TestMaxHpCalculationFormula()
        {
            ResetCharacterStats();
            Assert.AreEqual(39, _characterStats.MaxHP);
        }
    }
}