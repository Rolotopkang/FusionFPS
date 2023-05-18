using System;
using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using UnityEngine;

public class MysqlManager : Singleton<MysqlManager>
{
 //    public static string m_databaseIP = "113.31.168.31";
 //    public static string m_databasePort = "3306";
 //    public static string m_userID = "rgJ9eh6h5VK1fojJ";
 //    public static string m_password = "9sISdnHUbngg53eUYTMLimocvop4Peeh";
 //    public static string m_databaseName = "TOPFPS";
 //
 //    protected bool MysqlConnected = false;
 //    protected static string m_connectionString;                 // 数据库连接字符串
 //    public static MySqlConnection connection;
 //
 //    /// <summary>
	// /// 初始化数据库，利用字符串组拼方式来编写数据库的连接
	// /// </summary>
	// public void Init()
	// {
 //        m_connectionString = string.Format("Server = {0};" +
 //                                           " port = {1};" +
 //                                           " Database = {2};" +
 //                                           " User ID = {3};" +
 //                                           " Password = {4};" +
 //                                           " Pooling=true;" +
 //                                           " Charset = utf8;",
 //            m_databaseIP, m_databasePort, m_databaseName, m_userID, m_password);
	// 	//发送数据库连接字段 创建连接通道
	// 	using (connection = new MySqlConnection(m_connectionString))
	// 	{
	// 		try
	// 		{
	// 			//打开连接通道
	// 			connection.Open();
 //                Debug.Log("数据库打开成功");
 //                MysqlConnected = true;
 //            }
	// 		catch (MySqlException E)
	// 		{
 //                //如果有异常 则连接失败
 //                Debug.Log("数据库连接失败");
 //                UI_Error.GetInstance().OpenUI_NetWorkWarning();
 //                MysqlConnected = false;
	// 		}
	// 		finally
	// 		{
	// 			//关闭连接通道
	// 			connection.Close();
 //            }
	// 	}
 //        // Debug.Log(m_connectionString);
	// }
 //    
 //    
 //    /// <summary>
 //    /// 登录
 //    /// </summary>
 //    /// <param name="username">用户名</param>
 //    /// <param name="password">用户密码</param>
 //    public EnumTools.LoginState Login(string username, string password)
 //    {
 //        if (!MysqlConnected) return EnumTools.LoginState.Error ;
 //        
 //        try
 //        {
 //            connection.Open();
 //            string sqlQuary = "select * from User where username ='" + username + "' and password = '" + password + "'";
 //            MySqlCommand comd = new MySqlCommand(sqlQuary, connection);
 //            MySqlDataReader reader = comd.ExecuteReader();
 //            if (reader.Read())
 //            {
 //                return EnumTools.LoginState.Success;
 //            }
 //            else
 //            {
 //                reader.Close();
 //                sqlQuary = "select * from User where username ='" + username +"'";
 //                comd = new MySqlCommand(sqlQuary, connection);
 //                reader = comd.ExecuteReader();
 //                if (reader.Read())
 //                {
 //                    return EnumTools.LoginState.WrongPassword;
 //                }
 //                else
 //                {
 //                    return EnumTools.LoginState.SearchNoUser;
 //                }
 //            }
 //        }
 //        catch (System.Exception e)
 //        {
 //            MysqlConnected = false;
 //            Debug.Log(e.Message);
 //        }
 //        finally
 //        {
 //            connection.Close();
 //        }
 //        
 //        return EnumTools.LoginState.Error;
 //    }
 //    
 //    //注册验证
 //    public EnumTools.RegisterState Register(string username, string password)
 //    {
 //        if (!MysqlConnected) return EnumTools.RegisterState.Error;
 //        
 //        if (!(username.Length > 0 && password.Length > 0))
 //        {
 //            Debug.Log("-----输入用户名和密码------");
 //            return EnumTools.RegisterState.HasNoInput;
 //        }
 //        try
 //        {
 //            connection.Open();
 //            Debug.Log("-----连接成功！------");
 //            string sqlQuary = "select * from User where username =@paral1";
 //            MySqlCommand comd = new MySqlCommand(sqlQuary, connection);
 //            comd.Parameters.AddWithValue("paral1", username);
 //            MySqlDataReader reader = comd.ExecuteReader();
 //            if (reader.Read())
 //            {
 //                Debug.Log("-----用户名已存在，请重新输！------");
 //                return EnumTools.RegisterState.RepeatName;
 //            }
 //            else
 //            {
 //                reader.Close();
 //                sqlQuary = "insert into User(username,password) values ('"+username+"','"+password+"')";
 //                comd = new MySqlCommand(sqlQuary, connection);
 //                reader = comd.ExecuteReader();
 //                Debug.Log("-----插入成功！------");
 //                return EnumTools.RegisterState.Success;
 //            }
 //        }
 //        catch (System.Exception e)
 //        {
 //            MysqlConnected = false;
 //            Debug.Log(e.Message);
 //        }
 //        finally
 //        {
 //            connection.Close();
 //        }
 //
 //        return EnumTools.RegisterState.Error;
 //    }
 //
 //    private void OnApplicationQuit()
 //    {
 //        
 //    }
}
