require ("Dot/Config/ConfigSubSheet")
require ("Dot/Config/ConfigSheetLine")
local t_1 =function()
    return 1.0
end
local t_2 = {
    1,
    2,
    3,
    4,
}
local t_3 = {
    [1] = 1,
    [2] = 2,
}
local cofing_TestSheet_5 = {}
cofing_TestSheet_5.__index = cofing_TestSheet_5
setmetatable(cofing_TestSheet_5,ConfigSubSheet)
cofing_TestSheet_5.ids = {111,}
cofing_TestSheet_5[111] = {
    ID = 111,
    FloatValue = 1.5,
    BoolValue = false,
    ResValue_Index = 14,
    LuaValue = t_1,
    ArrayValue = t_2,
    DicValue = t_3,
}
cofing_TestSheet_5[111].__index = cofing_TestSheet_5[111]
setmetatable(cofing_TestSheet_5[111],ConfigSheetLine)

return cofing_TestSheet_5
