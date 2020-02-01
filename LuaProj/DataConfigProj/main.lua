
local main = function()
  
  local config = require("Game/Config/cofing_TestSheet")
  if not config then
    print("Error")
  else
    local t = config:GetAllId()
    for i=1, #t do
      local data = config:GetData(t[i])
      if data then 
        print(data.FloatValue)
      end
    end
  end
  
end

main()