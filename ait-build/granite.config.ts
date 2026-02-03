import { defineConfig } from '@apps-in-toss/web-framework/config';

//// SDK_GENERATED_START - DO NOT EDIT THIS SECTION ////
const sdkConfig = {
  appName: 'likelion-slime-runner',
  brand: {
    displayName: 'Slime Runner',
    primaryColor: '#3182F6',
    icon: 'https://static.toss.im/appsintoss/845/65199289-2125-4eb9-b90b-59b61d0b1396.png',
    bridgeColorMode: 'inverted',
  },
  webViewProps: {
    type: 'game',
    allowsInlineMediaPlayback: false,
    mediaPlaybackRequiresUserAction: false,
  },
  web: {
    host: process.env.AIT_VITE_HOST || 'localhost',
    port: parseInt(process.env.AIT_VITE_PORT || '5173', 10),
    strictPort: false,
    commands: {
      dev: 'vite --host',
      build: 'vite build',
    },
  },
  permissions: [],
  outdir: 'dist',
};
//// SDK_GENERATED_END ////

//// USER_CONFIG_START ////
const userConfig = {
  // 여기에 사용자 커스텀 설정을 추가하세요
};
//// USER_CONFIG_END ////

export default defineConfig({ ...sdkConfig, ...userConfig });
