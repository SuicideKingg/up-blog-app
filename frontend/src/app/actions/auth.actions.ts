import { createAction, props } from '@ngrx/store';

// Action triggered when a user logs in
export const loginSuccess = createAction(
  '[Auth] Login Success',
  props<{ username: string; token: string }>()
);

// Action triggered when a user logs out
export const logout = createAction('[Auth] Logout');
