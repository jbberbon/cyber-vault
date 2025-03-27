import * as zod from 'zod';
import {toTypedSchema} from '@vee-validate/zod';
export const loginSchema = toTypedSchema(
  zod.object({
    email: zod
      .string()
      .min(1, {message: 'Email is required'})
      .email({message: 'Must be a valid email'}),
    password: zod
      .string()
      .min(1, {message: 'Password is required'})
      .min(6, {message: 'Invalid credentials'}),
  })
);
