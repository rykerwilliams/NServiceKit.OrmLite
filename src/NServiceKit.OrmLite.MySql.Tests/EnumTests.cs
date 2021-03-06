﻿using System.Linq;
using NUnit.Framework;

namespace NServiceKit.OrmLite.MySql.Tests
{
    /// <summary>An enum tests.</summary>
    public class EnumTests : OrmLiteTestBase
    {
        /// <summary>Can create table.</summary>
        [Test]
        public void CanCreateTable()
        {
            OpenDbConnection().CreateTable<TypeWithEnum>(true);
        }

        /// <summary>Can store enum value.</summary>
        [Test]
        public void CanStoreEnumValue()
        {
            using(var con = OpenDbConnection())
            {
                con.CreateTable<TypeWithEnum>(true);
                con.Save(new TypeWithEnum {Id = 1, EnumValue = SomeEnum.Value1});
            }
        }

        /// <summary>Can get enum value.</summary>
        [Test]
        public void CanGetEnumValue()
        {
            using (var con = OpenDbConnection())
            {
                con.CreateTable<TypeWithEnum>(true);
                var obj = new TypeWithEnum { Id = 1, EnumValue = SomeEnum.Value1 };
                con.Save(obj);
                var target = con.GetById<TypeWithEnum>(obj.Id);
                Assert.AreEqual(obj.Id, target.Id);
                Assert.AreEqual(obj.EnumValue, target.EnumValue);
            }
        }

        /// <summary>Can query by enum value using select with expression.</summary>
        [Test]
        public void CanQueryByEnumValue_using_select_with_expression()
        {
            using (var con = OpenDbConnection())
            {
                con.CreateTable<TypeWithEnum>(true);
                con.Save(new TypeWithEnum { Id = 1, EnumValue = SomeEnum.Value1 });
                con.Save(new TypeWithEnum { Id = 2, EnumValue = SomeEnum.Value1});
                con.Save(new TypeWithEnum { Id = 3, EnumValue = SomeEnum.Value2 });

                var target = con.Select<TypeWithEnum>(q => q.EnumValue == SomeEnum.Value1);

                Assert.AreEqual(2, target.Count());
            }
        }

        /// <summary>Can query by enum value using select with string.</summary>
        [Test]
        public void CanQueryByEnumValue_using_select_with_string()
        {
            using (var con = OpenDbConnection())
            {
                con.CreateTable<TypeWithEnum>(true);
                con.Save(new TypeWithEnum { Id = 1, EnumValue = SomeEnum.Value1 });
                con.Save(new TypeWithEnum { Id = 2, EnumValue = SomeEnum.Value1 });
                con.Save(new TypeWithEnum { Id = 3, EnumValue = SomeEnum.Value2 });

                var target = con.Select<TypeWithEnum>("EnumValue = {0}", SomeEnum.Value1);

                Assert.AreEqual(2, target.Count());
            }
        }

        /// <summary>Can query by enum value using where with anon type.</summary>
        [Test]
        public void CanQueryByEnumValue_using_where_with_AnonType()
        {
            using (var con = OpenDbConnection())
            {
                con.CreateTable<TypeWithEnum>(true);
                con.Save(new TypeWithEnum { Id = 1, EnumValue = SomeEnum.Value1 });
                con.Save(new TypeWithEnum { Id = 2, EnumValue = SomeEnum.Value1 });
                con.Save(new TypeWithEnum { Id = 3, EnumValue = SomeEnum.Value2 });

                var target = con.Where<TypeWithEnum>(new { EnumValue = SomeEnum.Value1 });

                Assert.AreEqual(2, target.Count());
            }
        }
    }

    /// <summary>Values that represent SomeEnum.</summary>
    public enum SomeEnum : long
    {
        /// <summary>An enum constant representing the value 1 option.</summary>
        Value1 = 2147483648,

        /// <summary>An enum constant representing the value 2 option.</summary>
        Value2,

        /// <summary>An enum constant representing the value 3 option.</summary>
        Value3
    }

    /// <summary>A type with enum.</summary>
    public class TypeWithEnum
    {
        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>Gets or sets the enum value.</summary>
        /// <value>The enum value.</value>
        public SomeEnum EnumValue { get; set; } 
    }
}
