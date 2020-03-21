/*The file was created by tool.
-----------------------------------------------
Please don't change it manually!!!
Please don't change it manually!!!
Please don't change it manually!!!
-----------------------------------------------
*/


namespace Game.Net.Proto
{
    public static class C2SProto_Parser
    {
        public static void RegisterParser(Dot.Net.Server.ServerNetListener serverNetListener)
        {
            serverNetListener.RegisterParser(C2SProto.C2S_LOGIN,Parse_LoginRequest);
            serverNetListener.RegisterParser(C2SProto.C2S_SHOP_LIST,Parse_ShopListRequest);
        }

        private static object Parse_LoginRequest(int netID,int messageID,byte[] msgBytes)
        {
            return LoginRequest.Parser.ParseFrom(msgBytes);
        }
        private static object Parse_ShopListRequest(int netID,int messageID,byte[] msgBytes)
        {
            return ShopListRequest.Parser.ParseFrom(msgBytes);
        }
    }
}
