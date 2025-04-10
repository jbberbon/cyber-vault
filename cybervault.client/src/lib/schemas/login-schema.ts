import * as zod from 'zod';
import {toTypedSchema} from '@vee-validate/zod';
export const loginSchema = toTypedSchema(
  zod.object({
    email: zod
      .string({message: 'Email is required'})
      .max(150, {message: 'Email should be less than 150 characters'})
      .email({message: 'Must be a valid email.'}),
    password: zod
      .string({message: 'Password is required'})
      .min(8, {message: 'Password must contain at least 8 characters'}),
  })
);
