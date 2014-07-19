package Server;

import java.sql.*;

public class MySQL {
	private Connection connect = null;
	private Statement statement = null;
	private ResultSet resultSet = null;
	private String host;
	private int port;
	private String username;
	private String password;
	private String dbName;
	public MySQL(String host, int port, String username, String password, String dbName) {
		this.host = host;
		this.port = port;
		this.username = username;
		this.password = password;
		this.dbName = dbName;
        String connectionString = ("jdbc:mysql://" + host + ":" + port + "/" + dbName + "?user=" + username + "&password=" + password);
        
        try {
        	Class.forName("com.mysql.jdbc.Driver");
			connect = (Connection)DriverManager.getConnection(connectionString);
		} catch (SQLException e) {
			// TODO Auto-generated catch block
			System.err.println("Error while initializing MySQL...: " + e.toString());
		} catch (ClassNotFoundException e) {
			// TODO Auto-generated catch block
			System.err.println("Error while initializing MySQL...: " + e.toString());
		}
	}
	
	public MySQL clone() {
		return new MySQL(host, port, username, password, dbName);
	}

	public void Query(String queryString) {
		try {
			if (resultSet != null)
				resultSet.close();
			if (statement != null)
				statement.close();
			// Statements allow to issue SQL queries to the database
			statement = connect.createStatement();
			// Result set get the result of the SQL query
			resultSet = statement.executeQuery(queryString);
		} catch (SQLException e) {
			// TODO Auto-generated catch block
			System.err.println("Error while Query MySQL...: " + e.toString());
		}
	}

    public void QueryNotExecute(String queryString) {
		try {
			if (resultSet != null)
				resultSet.close();
			if (statement != null)
				statement.close();
			// Statements allow to issue SQL queries to the database
			statement = connect.createStatement();
			// Result set get the result of the SQL query
			statement.executeUpdate(queryString);
		} catch (SQLException e) {
			// TODO Auto-generated catch block
			System.err.println("Error while QueryNotExecute MySQL...: " + e.toString());
		}
    }

	public ResultSet getResultSet() {
		return resultSet;
	}

	public void close() {
		try {
			if (resultSet != null)
				resultSet.close();
			if (statement != null)
				statement.close();
			if (connect != null)
				connect.close();
		} catch (Exception e) {
			System.out.println(e.toString());
		}
	}
}
