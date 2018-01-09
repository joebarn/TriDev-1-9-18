(module

	(func $print (import "imports" "print") (param i32))
	
	(func $main

		i32.const 5
		i32.const 6
		i32.eq
		
		if
			i32.const 1
			call $print
		else
			i32.const 0
			call $print
		end
	
	)
	
	
	(export "main" (func $main))



)