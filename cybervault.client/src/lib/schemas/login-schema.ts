import * as zod from 'zod';
import {toTypedSchema} from '@vee-validate/zod';
export const loginSchema = toTypedSchema(
  zod.object({
    email: zod
      .string()
      .min(1, {message: 'Email is required.'})
      .max(150, {message: 'Email should be less than 150 char.'})
      .email({message: 'Must be a valid email.'}),
    password: zod
      .string()
      .min(6, {message: 'Invalid credentials.'}),
  })
);
