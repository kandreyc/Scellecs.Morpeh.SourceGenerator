using Scellecs.Morpeh.SourceGenerator.Aspect;

namespace Tests.AspectGenerator;

public partial struct Aspect2 : IAspect<Test1, Test2> { }

public partial struct Aspect3 : IAspect<Test1, Test2, Test3> { }

public partial struct Aspect4 : IAspect<Test1, Test2, Test3, Test4> { }

public partial struct Aspect5 : IAspect<Test1, Test2, Test3, Test4, Test5> { }

public partial struct Aspect6 : IAspect<Test1, Test2, Test3, Test4, Test5, Test6> { }

public partial struct Aspect7 : IAspect<Test1, Test2, Test3, Test4, Test5, Test6, Test7> { }

public partial struct Aspect8 : IAspect<Test1, Test2, Test3, Test4, Test5, Test6, Test7, Test8> { }

public partial struct Aspect9 : IAspect<Test1, Test2, Test3, Test4, Test5, Test6, Test7, Test8, Test9> { }

public partial struct Aspect10 : IAspect<Test1, Test2, Test3, Test4, Test5, Test6, Test7, Test8, Test9, Test10> { }