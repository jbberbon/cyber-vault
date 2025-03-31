import type {Router} from "vue-router";

export const navigateBreadcrumb = async (router: Router, path?: string) => {
  if (!path) {
    await router.push('/my-vault');
  } else {
    await router.push({name: "my-vault-subdirectory", params: {id: path}});
  }
};
