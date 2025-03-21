import { createReducer, on } from '@ngrx/store';
import { loginSuccess, logout } from './auth.actions';
import { AuthState } from '../models/abstraction/auth-state.model';

export const initialState: AuthState = { username: '', token: '' };

export const authReducer = createReducer(
  initialState,
  on(loginSuccess, (state, { username, token }) => ({ username, token })),
  on(logout, () => initialState)
);