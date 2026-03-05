\ TodoMVC model implementation for gforth.
\ Supports add, edit, toggle, destroy, filter, and clear-completed.

256 constant max-todos
128 constant max-title

create todo-ids max-todos cells allot
create todo-done-flags max-todos cells allot
create todo-title-lens max-todos cells allot
create todo-titles max-todos max-title * allot

variable todo-count
variable next-id
variable filter-mode \ 0=all, 1=active, 2=completed

: init-todomvc ( -- )
  0 todo-count !
  1 next-id !
  0 filter-mode !
;

: title-addr ( idx -- addr )
  max-title * todo-titles +
;

: todo-id@ ( idx -- id )
  cells todo-ids + @
;

: todo-id! ( id idx -- )
  cells todo-ids + !
;

: todo-done@ ( idx -- f )
  cells todo-done-flags + @
;

: todo-done! ( f idx -- )
  cells todo-done-flags + !
;

: todo-len@ ( idx -- u )
  cells todo-title-lens + @
;

: todo-len! ( u idx -- )
  cells todo-title-lens + !
;

: set-title ( c-addr u idx -- )
  >r
  max-title min dup r@ todo-len!
  r@ title-addr swap move
  rdrop
;

: title@ ( idx -- c-addr u )
  dup title-addr swap todo-len@
;

: find-index-by-id ( id -- idx found? )
  todo-count @ 0 do
    i todo-id@ over = if
      drop i true unloop exit
    then
  loop
  drop -1 false
;

: add-todo ( c-addr u -- id )
  todo-count @ max-todos >= abort" todo capacity exceeded"
  todo-count @ >r
  next-id @ r@ todo-id!
  false r@ todo-done!
  r@ set-title
  r> drop
  todo-count @ 1+ todo-count !
  next-id @ 1+ next-id !
  next-id @ 1-
;

: toggle-todo ( id -- found? )
  find-index-by-id 0= if
    drop false exit
  then
  dup todo-done@ 0= swap todo-done!
  true
;

: edit-todo ( id c-addr u -- found? )
  rot find-index-by-id 0= if
    drop 2drop false exit
  then
  set-title
  true
;

: shift-left-from ( idx -- )
  todo-count @ 1- over > if
    todo-count @ 1- swap do
      i 1+ todo-id@ i todo-id!
      i 1+ todo-done@ i todo-done!
      i 1+ todo-len@ i todo-len!
      i 1+ title-addr i title-addr max-title move
    loop
  else
    drop
  then
;

: destroy-todo ( id -- found? )
  find-index-by-id 0= if
    drop false exit
  then
  shift-left-from
  todo-count @ 1- todo-count !
  true
;

: clear-completed ( -- removed )
  0 { removed }
  0 { idx }
  begin idx todo-count @ < while
    idx todo-done@ if
      idx shift-left-from
      todo-count @ 1- todo-count !
      removed 1+ to removed
    else
      idx 1+ to idx
    then
  repeat
  removed
;

: completed-count ( -- n )
  0
  todo-count @ 0 do
    i todo-done@ if 1+ then
  loop
;

: active-count ( -- n )
  todo-count @ completed-count -
;

: set-filter ( n -- )
  dup 0 3 within 0= abort" invalid filter"
  filter-mode !
;

: visible? ( idx -- f )
  filter-mode @ case
    0 of drop true endof
    1 of todo-done@ 0= endof
    2 of todo-done@ endof
  endcase
;

: visible-count ( -- n )
  0
  todo-count @ 0 do
    i visible? if 1+ then
  loop
;

: .todo-line ( idx -- )
  dup todo-id@ . space
  dup todo-done@ if ." [x] " else ." [ ] " then
  title@ type cr
;

: list-visible ( -- shown )
  ." " cr
  0
  todo-count @ 0 do
    i visible? if
      i .todo-line
      1+
    then
  loop
;

: .filter ( -- )
  filter-mode @ case
    0 of ." all" endof
    1 of ." active" endof
    2 of ." completed" endof
  endcase
;

: .stats ( -- )
  ." total=" todo-count @ .
  ."  active=" active-count .
  ."  completed=" completed-count .
  ."  filter=" .filter cr
;

: manual ( -- )
  ." TodoMVC gforth manual" cr
  ." init-todomvc               reset all state" cr
  ." add-todo                   usage: ( c-addr u -- id )" cr
  ." id toggle-todo .           toggle completed flag" cr
  ." edit-todo                  usage: ( id c-addr u -- found? )" cr
  ." id destroy-todo .          remove item" cr
  ." clear-completed .          remove completed items, print count" cr
  ." n set-filter               0=all 1=active 2=completed" cr
  ." list-visible .             print visible items, print shown count" cr
  ." .stats                     print totals and filter" cr
;

init-todomvc
