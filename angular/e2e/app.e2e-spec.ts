import { BoilerplateProjectTemplatePage } from './app.po';

describe('BoilerplateProject App', function() {
  let page: BoilerplateProjectTemplatePage;

  beforeEach(() => {
    page = new BoilerplateProjectTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
