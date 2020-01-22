require ("Dot/Config/ConfigSubSheet")
require ("Dot/Config/ConfigSheetLine")
local t_1 =function()
    return 1.0
end
local t_2 = {
    1,
    2,
    3,
}
local t_3 = {
    [1] = 1,
    [2] = 2,
}
local cofing_TestSheet_4 = {}
cofing_TestSheet_4.__index = cofing_TestSheet_4
setmetatable(cofing_TestSheet_4,ConfigSubSheet)
cofing_TestSheet_4.ids = {31,32,54,}
cofing_TestSheet_4[31] = {
    ID = 31,
    LongValue = 754,
    LuaValue = t_1,
    ArrayValue = t_2,
    DicValue = t_3,
}
cofing_TestSheet_4[31].__index = cofing_TestSheet_4[31]
setmetatable(cofing_TestSheet_4[31],ConfigSheetLine)

cofing_TestSheet_4[32] = {
    ID = 32,
    LongValue = 2,
    FloatValue = 1.3,
    BoolValue = false,
    StringValue_Index = 7,
    LuaValue = t_1,
    ArrayValue = t_2,
    DicValue = t_3,
}
cofing_TestSheet_4[32].__index = cofing_TestSheet_4[32]
setmetatable(cofing_TestSheet_4[32],ConfigSheetLine)

cofing_TestSheet_4[54] = {
    ID = 54,
    LongValue = 4,
    FloatValue = 1.6,
    BoolValue = false,
    ResValue_Index = 13,
    LuaValue = t_1,
    ArrayValue = t_2,
    DicValue = t_3,
}
cofing_TestSheet_4[54].__index = cofing_TestSheet_4[54]
setmetatable(cofing_TestSheet_4[54],ConfigSheetLine)

return cofing_TestSheet_4
