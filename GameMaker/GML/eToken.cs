namespace GameMaker.GML
{
	public enum eToken
	{
		eEOF = -2,
		eError = -1,
		eName = 0,
		eNumber = 1,
		eString = 2,
		eConstant = 5,
		eFunction = 6,
		eVariable = 7,
		eVariableSimple = 8,
		eGlobal = 9,
		eBegin = 11,
		eEnd = 12,
		eIf = 13,
		eThen = 14,
		eElse = 0xF,
		eWhile = 0x10,
		eDo = 17,
		eFor = 18,
		eUntil = 19,
		eRepeat = 20,
		eExit = 21,
		eBreak = 22,
		eContinue = 23,
		eWith = 24,
		eReturn = 25,
		eSwitch = 26,
		eCase = 27,
		eDefault = 28,
		eVar = 29,
		eGlobalVar = 30,
		eCaseConstant = 0x1F,
		eAssign = 101,
		eAssignPlus = 102,
		eAssignMinus = 103,
		eAssignTimes = 104,
		eAssignDivide = 105,
		eOpen = 106,
		eClose = 107,
		eSepStatement = 108,
		eSepArgument = 109,
		eArrayOpen = 110,
		eArrayClose = 111,
		eDot = 112,
		eLabel = 113,
		eAssignOr = 114,
		eAssignAnd = 115,
		eAssignXor = 116,
		eAnd = 201,
		eOr = 202,
		eNot = 203,
		eLess = 204,
		eLessEqual = 205,
		eEqual = 206,
		eNotEqual = 207,
		eGreaterEqual = 208,
		eGreater = 209,
		ePlus = 210,
		eMinus = 211,
		eTime = 212,
		eDivide = 213,
		eDiv = 214,
		eMod = 215,
		eXor = 216,
		eBitOr = 217,
		eBitAnd = 218,
		eBitXor = 219,
		eBitNegate = 220,
		eBitShiftLeft = 221,
		eBitShiftRight = 222,
		eBlock = 1000,
		eUnary = 1010,
		eBinary = 1011
	}
}
