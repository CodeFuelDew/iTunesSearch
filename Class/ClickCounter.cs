using System;


public class ClickCounter {

    public static Dictionary<string,int>? Counter {
        get;set;
    }

    public static void handle_click(string ce) {
       Counter ??= [];

       if(!Counter.ContainsKey(ce)) 
            Counter.Add(ce,1);
        else
            Counter[ce] = Counter[ce] + 1;
       
    }
}