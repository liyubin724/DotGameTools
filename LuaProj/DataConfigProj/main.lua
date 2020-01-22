local main = function()
  
  local config = require("Game/Config/cofing_TestSheet")
  if not config then
    print("Error")
  else
    local t = config:GetIds()
    for _, value in ipairs(t) do
      print(value)
    end
  end
  
end

main()