using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockManager
{
    interface StockManagerInterface
    {
        bool updateStock(String ticker);
	
	    stock getStockFromDB(String ticker);

	    bool isStockInDB(String ticker);
	
	    void addStockToDB(String ticker);
	
	    bool removeStockFromDB(String ticker);
	
	    bool isValidTicker(String ticker);
	
	    List<string> getStocksInDB();
	
	    stock getStockFromWeb(String ticker);
	
	    string getStockNameFromDB(String ticker);
	
	    string getStockNameFromWeb(String ticker);

        string getStockName(String ticker);
    }
}
