#! /usr/bin/env gforth

require ../docs/forth-tap-0.0.1/lib/test-more.fs
require ./todomvc.fs

28 plan

\ given a clean todo model
init-todomvc

\ when adding two todos
s" buy milk" add-todo 1 = ok
s" write tests" add-todo 2 = ok

\ then ids and counts are correct
todo-count @ 2 = ok
active-count 2 = ok
completed-count 0 = ok

\ when toggling first todo to completed
1 toggle-todo ok
\ then completed/active counts change
completed-count 1 = ok
active-count 1 = ok

\ when toggling first todo back to active
1 toggle-todo ok
\ then completed count returns to zero
completed-count 0 = ok

\ when editing first todo title
1 s" buy oat milk" edit-todo ok
\ then todo exists and title is updated
1 find-index-by-id nip ok
1 find-index-by-id drop title@ s" buy oat milk" compare 0= ok

\ when toggling or destroying missing ids
99 toggle-todo 0= ok
99 destroy-todo 0= ok

\ when destroying second todo
2 destroy-todo ok
\ then one todo remains
todo-count @ 1 = ok

\ when adding two more and completing one
s" done one" add-todo 3 = ok
s" keep one" add-todo 4 = ok
3 toggle-todo ok

\ when clearing completed todos
clear-completed 1 = ok
\ then only active todos remain
todo-count @ 2 = ok
completed-count 0 = ok
active-count 2 = ok

\ when changing filter to all/active/completed
0 set-filter
\ then all todos are visible
visible-count 2 = ok
1 set-filter
\ then active todos are visible
visible-count 2 = ok
2 set-filter
\ then no completed todos are visible
visible-count 0 = ok

\ when invoking manual under catch
\ then no exception is thrown
' manual catch 0= ok

bye
