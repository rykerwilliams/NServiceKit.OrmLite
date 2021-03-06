using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Data;

using NServiceKit.Common.Utils;
using NServiceKit.DataAnnotations;
using NServiceKit.Common.Extensions;
using System.Reflection;

using NServiceKit.OrmLite;
using NServiceKit.OrmLite.Firebird;

namespace TestLiteFirebird01
{
	
	[Alias("USERS")]
	public  class User
	{
		
		[Alias("ID")]
		[Sequence("USERS_ID_GEN")]
		public int Id { get; set; }
		[Alias("NAME")]
    	public string Name { get; set; }
		[Alias("PASSWORD")]  
    	public string Password { get; set; }
		[Alias("COL1")]
    	public string Col1 { get; set; }
		[Alias("COL2")]
		public string Col2 { get; set; }
		[Alias("COL3")]
		public string Col3 { get; set; }
		
		[Alias("ACTIVEINTEGER")]
		public bool Active { get; set; }
		
		[Alias("ACTIVECHAR")]
		public bool Active2 { get; set; }
		
		[Ignore]
		public string SomeStringProperty { 
			get{ return "SomeValue No from dB!!!";}
		}
		
		[Ignore]
		public Int32 SomeInt32Property { 
			get{ return 35;}
		}
		
		[Ignore]
		public DateTime SomeDateTimeProperty { 
			get{ return DateTime.Now ;}
		}
		
		[Ignore]
		public Int32? SomeInt32NullableProperty { 
			get{ return null;}
		}
		
		[Ignore]
		public DateTime? SomeDateTimeNullableProperty { 
			get{ return null ;}
		}
		
		
		
	}
	
	class MainClass
	{
		public static void Main (string[] args)
		{
			
			
			//Set one before use (i.e. in a static constructor).
			
			OrmLiteConfig.DialectProvider = new FirebirdOrmLiteDialectProvider();
									
			using (IDbConnection db =
			       "User=SYSDBA;Password=masterkey;Database=employee.fdb;DataSource=localhost;Dialect=3;charset=ISO8859_1;".OpenDbConnection())
			using ( IDbCommand dbConn = db.CreateCommand())
			{
				//try{
					
					
    				dbConn.Insert(new User 
					{ 	
						Name= string.Format("Hello, World! {0}", DateTime.Now),
						Password="jkkoo",
						Col1="01",
						Col2="02",
						Col3="03"
							
					});
					
					User user = new User(){
						Name="New User ",
						Password= "kka",
						Col1="XX",
						Col2="YY",
						Col3="ZZ",
						Active=true
					};
					
					
					dbConn.Insert(user);
					
					Console.WriteLine("++++++++++Id for {0} {1}",user.Name,  user.Id);
						
					
    				var rows = dbConn.Select<User>();
					
					Console.WriteLine("++++++++++++++records in users {0}", rows.Count);
					foreach(User u in rows){
						Console.WriteLine("{0} -- {1} -- {2} -- {3} -{4} --{5} ", u.Id, u.Name, u.SomeStringProperty, u.SomeDateTimeProperty,
						                  (u.SomeInt32NullableProperty.HasValue)?u.SomeDateTimeNullableProperty.Value.ToString(): "",
						                  u.Active);						
						dbConn.Delete(u);
					}
					
					rows = dbConn.Select<User>();
					
					Console.WriteLine("-------------records in users after delete {0}", rows.Count);
					
				//}	
				
				//catch(Exception e){
				//	Console.WriteLine(e);
				//}
			}

		}
	}
}

