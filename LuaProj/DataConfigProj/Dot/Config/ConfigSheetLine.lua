require ("Dot/Config/ConfigConst")

local rawget = rawget
local rawset = rawset
local ConfigConst = ConfigConst
local string = string

ConfigSheetLine = {}

function ConfigSheetLine:SetDepend(depends)
  rawset(self,ConfigConst.DEPENDS_NAME,depends)
end

function ConfigSheetLine:GetValue(k)
  local value = rawget(self,k)
  
  if value then
    return value
  end
  
  value = self:GetDefaultValue(k)
  if value then
    return value
  end
  
  value = self:GetStrValue(k)
  if value then
    return value
  end
  
  value = self:GetTextValue(k)
  if value then
    return value
  end
  
  return nil
  
end

function ConfigSheetLine:GetDefaultValue(k)
  local depends = rawget(self,ConfigConst.DEPENDS_NAME)
  if not depends then
    return nil
  end
  
  local defaultValues = depends[ConfigConst.DEFAULT_VALUE_NAME]
  return defaultValues[k]
end

function ConfigSheetLine:GetStrValue(k)
  local depends = rawget(self,ConfigConst.DEPENDS_NAME)
  if not depends then
    return nil
  end
  
  local strFieldNames = depends[ConfigConst.STR_FIELD_NAME]
  if not strFieldNames then
    return nil
  end
  
  for i=1, #strFieldNames do
    local field = strFieldNames[i]
  	if field == k then
  	 local strIndex = self:GetValue(string.format(ConfigConst.FIELD_INDEX_FORMAT,k))
  	 if not strIndex then
      return nil
  	 else
  	   local strValues = depends[ConfigConst.STR_VALUE_NAME]
  	   if strValues then
  	     return strValues[strIndex]
  	   else
  	     return nil
  	   end
  	 end
  	end
  end
  
  return nil
end

function ConfigSheetLine:GetTextValue(k)
  local depends = rawget(self,ConfigConst.DEPENDS_NAME)
  if not depends then
    return nil
  end
  
  local textFieldNames = depends[ConfigConst.TEXT_FIELD_NAME]
  if not textFieldNames then
    return nil
  end
  
  for i=1, #textFieldNames do
    local field = textFieldNames[i]
  	if field == k then
  	 local textIndex = self:GetValue(string.format(ConfigConst.FIELD_INDEX_FORMAT,k))
  	 if not textIndex then
  	   return nil
  	 else
  	   local text = depends[ConfigConst.TEXT_NAME]
  	   return text:GetText(textIndex)
  	 end
  	end
  end
  return nil
end

ConfigSheetLine.__index = function(t,k)
  local result = ConfigSheetLine[k]
  if result then
    return result
  end
  
  return ConfigSheetLine.GetValue(t,k)
end

ConfigSheetLine.__newindex = function(t,k,v)
  error("Read only,so you can't add new key for it")
end
