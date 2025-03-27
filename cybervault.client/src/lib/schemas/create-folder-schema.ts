import * as zod from 'zod';
import {toTypedSchema} from '@vee-validate/zod';

export const createFolderSchema = toTypedSchema(
  zod.object({
    folderName: zod
      .string()
      .min(1, {message: 'Folder name is required'})
      .regex(/^(?!.*[+/\\]).*$/, {message: 'Folder name cannot contain +, /, or \\'})
  })
);
