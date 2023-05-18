// using ConsoleBlockChainApp;
//
// namespace BlockChainApp.Services;
//
// public class AppContainer
// {
//     private static SimpleApp _instance;
//     private static readonly object _lockObject = new object();
//
//     // Private constructor to prevent instantiation from outside the class
//     private AppContainer()
//     {
//         // Initialization code
//     }
//
//     // Public method to access the singleton instance
//     public static SimpleApp GetInstance()
//     {
//         if (_instance == null)
//         {
//             lock (_lockObject)
//             {
//                 // Double-check locking to ensure thread safety
//                 if (_instance == null)
//                 {
//                     _instance = new SimpleApp();
//                 }
//             }
//         }
//
//         return _instance;
//     }
// }