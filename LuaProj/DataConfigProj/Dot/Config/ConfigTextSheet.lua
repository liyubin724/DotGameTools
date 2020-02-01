ConfigTextSheet = {}

function ConfigTextSheet:GetText(textId)
  local data = self[textId]
  if not data then
    return nil
  end
  
  return data.zh
end

ConfigTextSheet.__index = function(t,k)
  if not t or type(t) ~= "table" then
    error("arg error")
  end
  
  local func = ConfigTextSheet[k]
  if not func then
    error("func not found")
  end
  
  return func
end

ConfigTextSheet.__newindex = function(t,k,v)
  error("Read only,so you can't add new key for it")
end