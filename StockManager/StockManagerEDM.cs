using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using StockManager;
using System.Data;

namespace StockManager
{
    class StockManagerEDM : StockManagerInterface
    {
        private stocksEntities Store;
        private string ConnectionString;
        
        public StockManagerEDM(string username, string password, string database, string ipAddress)
        {
            ConnectionString = "metadata=res://*/Model1.csdl|res://*/Model1.ssdl|res://*/Model1.msl;provider=MySql.Data.MySqlClient;provider connection string=&quot;server=" + ipAddress + ";User Id=" + username + ";password=" + password + ";database=" + database + ";Persist Security Info=True&quot;";

            Store = new stocksEntities(ConnectionString);
            if (Store.Connection.State == System.Data.ConnectionState.Broken || Store.Connection.State == System.Data.ConnectionState.Closed)
                throw new AggregateException("Connection to database failed.");
            
        }

        public bool updateStock(string ticker)
        {
            throw new NotImplementedException();
        }

        public stock getStockFromDB(string ticker)
        {
            throw new NotImplementedException();
        }

        public bool isStockInDB(string ticker)
        {
            return 1 >= Store.stocks.Count(x => x.ticker.Equals(ticker, StringComparison.CurrentCultureIgnoreCase));
        }

        public void addStockToDB(string ticker)
        {
            if (isStockInDB(ticker))
            {
                updateStock(ticker);
                return;
            }
            
            HttpWebRequest checkTicker = (HttpWebRequest)WebRequest.Create("http://finance.yahoo.com/d/quotes.csv?s=" + ticker + "&f=n");

            WebResponse wr = checkTicker.GetResponse();

            stock newStock = new stock();
            newStock.name = wr.ToString().Trim(" \"".ToCharArray());
            newStock.ticker = ticker;

            <stockdata> newStockData = new List<stockdata>();

            newStock.stockdatas 
		
		PreparedStatement stockIndex = conn.prepareStatement("insert into stocklist (stock, name) values(?,?)");
		stockIndex.setString(1, ticker);
		stockIndex.setString(2, b2.readLine());
		stockIndex.execute();
		stockIndex.close();
		b2.close();
		
		
		DateTime datetime = new DateTime();
		
		URL stockURL = new URL("http://ichart.finance.yahoo.com/table.csv?s=" + ticker + "&a=0&b=01&c=1900&d=" + Integer.toString(datetime.getMonthOfYear() - 1) + "&e=" + Integer.toString(datetime.getDayOfMonth()) + "&f=" + Integer.toString(datetime.getYear()) + "&g=d&ignore=.csv");
		URLConnection stockConn = stockURL.openConnection();
		InputStreamReader i = new InputStreamReader(stockConn.getInputStream());
		BufferedReader b = new BufferedReader(i);
		PreparedStatement insertMe = conn.prepareStatement("insert into stockdata (stock,date,open,high,low,close,volume,adjclose) VALUES(?,?,?,?,?,?,?,?)");
		String raw = b.readLine();
		System.out.println(raw);
		while(b.ready())
		{
			
			raw = b.readLine();
			//System.out.println(raw);
			String[] in = raw.split(",");
			if(in.length != 7) throw new Exception("Wrong number of columns in result");
			
			insertMe.setString(1, ticker);
			insertMe.setDate(2, Date.valueOf(in[0]));
			insertMe.setBigDecimal(3, new BigDecimal(in[1]));
			insertMe.setBigDecimal(4, new BigDecimal(in[2]));
			insertMe.setBigDecimal(5, new BigDecimal(in[3]));
			insertMe.setBigDecimal(6, new BigDecimal(in[4]));
			insertMe.setLong(7, Long.parseLong(in[5]));
			insertMe.setBigDecimal(8, new BigDecimal(in[6]));
			System.out.println("Trying insert " + in[0] + ": " + (insertMe.executeUpdate() == 0?"Fail":"Success"));
		}
		
		insertMe.close();
		i.close();
		b.close();
		

            return;
        }

        public bool removeStockFromDB(string ticker)
        {
            if (!isStockInDB(ticker)) return true;
            stock Stock = (from st in Store.stocks where st.ticker.Equals(ticker, StringComparison.CurrentCultureIgnoreCase) select st).First();
            foreach (stockdata sd in Stock.stockdatas)
            {
                Store.stockdatas.DeleteObject(sd);
            }
            Store.stocks.DeleteObject(Stock);

            if (1 <= Store.SaveChanges()) return true;
            return false;
        }

        public bool isValidTicker(string ticker)
        {
            if (isStockInDB(ticker)) return true;

            HttpWebRequest checkTicker = (HttpWebRequest)WebRequest.Create("http://finance.yahoo.com/d/quotes.csv?s=" + ticker + "&f=v");

            WebResponse wr = checkTicker.GetResponse();

            return !wr.ToString().Equals("N/A", StringComparison.Ordinal);
        }

        public List<string> getStocksInDB()
        {
            try
            {
                List<string> list = (from st in Store.stocks where true select st.ticker) as List<string>;
                return list;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return null;
            }
        }

        public stock getStockFromWeb(string ticker)
        {
            addStockToDB(ticker);
            return getStockFromDB(ticker);
        }

        public string getStockNameFromDB(string ticker)
        {
            string name = (from st in Store.stocks where st.ticker.Equals(ticker, StringComparison.CurrentCultureIgnoreCase) select st.name).First();

            return name;
        }

        public string getStockNameFromWeb(string ticker)
        {
            throw new NotImplementedException();
        }

        public string getStockName(string ticker)
        {
            throw new NotImplementedException();
        }
    }
}
